using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyShippingPlatform.Domain.Common;
using MyShippingPlatform.Domain.Enums;

namespace MyShippingPlatform.Domain.Entities;

public class AIAuditRecord : BaseEntity
{
    // Foreign key for Shipment
    public Guid ShipmentId { get; set; }

    // AI Audit Extraction Results
    public string ExtractedHsCode { get; set; } = string.Empty;
    public decimal DeclaredValue { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public string RiskReasoning { get; set; } = string.Empty;

    // Raw output from Python Agent (Stored as JSON string for future model evaluations)
    public string RawAgentOutputJson { get; set; } = string.Empty;

    // Human-in-the-Loop Audit Trail
    public bool IsReviewedByHuman { get; set; } = false;
    public string? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Shipment Shipment { get; set; } = null!;
}
