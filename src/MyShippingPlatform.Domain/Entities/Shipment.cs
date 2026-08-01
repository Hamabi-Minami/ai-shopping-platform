using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyShippingPlatform.Domain.Common;
using MyShippingPlatform.Domain.Enums;
using MyShippingPlatform.Domain.Events;

namespace MyShippingPlatform.Domain.Entities;

public class Shipment : BaseEntity
{
    // Core Domain Fields
    public string TrackingNumber { get; set; } = string.Empty;
    public string ShipperName { get; set; } = string.Empty;
    public string ConsigneeName { get; set; } = string.Empty;
    public string OriginCountry { get; set; } = string.Empty;
    public string DestinationCountry { get; set; } = string.Empty;

    // Encapsulated State Machine (Private Setter to enforce domain rules)
    public ShipmentStatus Status { get; private set; } = ShipmentStatus.Draft;

    // Multi-Tenant Isolation
    public string TenantId { get; set; } = "default-tenant";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // 1:N Relationship
    public ICollection<AIAuditRecord> AuditRecords { get; private set; } = new List<AIAuditRecord>();

    // -------------------------------------------------------------
    // Domain Methods (Business Rules & Status Transitions)
    // -------------------------------------------------------------

    /// <summary>
    /// Mark shipment as being processed by AI Agent
    /// </summary>
    public void MarkAsAuditing()
    {
        if (Status == ShipmentStatus.AiApproved || Status == ShipmentStatus.HumanApproved)
        {
            throw new InvalidOperationException("Cannot re-audit an already approved shipment.");
        }

        Status = ShipmentStatus.Auditing;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Apply AI Agent extraction result and evaluate risk status
    /// </summary>
    public void ApplyAuditResult(AIAuditRecord auditRecord)
    {
        AuditRecords.Add(auditRecord);

        // State Machine Rule: High risk flags the shipment, otherwise auto-approve via AI
        Status = auditRecord.RiskLevel == RiskLevel.High
            ? ShipmentStatus.Flagged
            : ShipmentStatus.AiApproved;

        UpdatedAt = DateTime.UtcNow;

        // Raise Domain Event
        AddDomainEvent(new ShipmentAuditedEvent(Id, auditRecord.RiskLevel, auditRecord.ExtractedHsCode));
    }

    /// <summary>
    /// Human-in-the-loop manual approval with RBAC Permission Check
    /// </summary>
    public void HumanApprove(string userId, IEnumerable<string> userPermissions)
    {
        // 1. RBAC Permission Rule
        if (!userPermissions.Contains("shipment:approve"))
        {
            throw new UnauthorizedAccessException("User lacks the required 'shipment:approve' permission.");
        }

        // 2. Business Rule: Only flagged or auditing shipments require manual approval
        if (Status != ShipmentStatus.Flagged && Status != ShipmentStatus.Auditing)
        {
            throw new InvalidOperationException($"Shipment in status '{Status}' cannot be manually approved.");
        }

        Status = ShipmentStatus.HumanApproved;

        var latestAudit = AuditRecords.OrderByDescending(a => a.CreatedAt).FirstOrDefault();
        if (latestAudit != null)
        {
            latestAudit.IsReviewedByHuman = true;
            latestAudit.ReviewedByUserId = userId;
            latestAudit.ReviewedAt = DateTime.UtcNow;
        }

        UpdatedAt = DateTime.UtcNow;

        // Raise Domain Event
        AddDomainEvent(new ShipmentApprovedEvent(Id, userId));
    }

    /// <summary>
    /// Executes automated approval when AI Agent validation passes.
    /// </summary>
    public void SystemApprove(string reason)
    {
        Status = ShipmentStatus.AiApproved;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Flags shipment for manual human review when AI Agent detects risks or fails.
    /// </summary>
    public void FlagForReview(string reason)
    {
        Status = ShipmentStatus.Flagged;
        UpdatedAt = DateTime.UtcNow;
    }
}