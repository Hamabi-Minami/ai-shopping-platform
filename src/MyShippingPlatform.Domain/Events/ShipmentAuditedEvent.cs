using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyShippingPlatform.Domain.Common;
using MyShippingPlatform.Domain.Enums;

namespace MyShippingPlatform.Domain.Events;

public record ShipmentAuditedEvent(
    Guid ShipmentId,
    RiskLevel RiskLevel,
    string HsCode
) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}