using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyShippingPlatform.Domain.Common;

namespace MyShippingPlatform.Domain.Events;

public record ShipmentApprovedEvent(
    Guid ShipmentId,
    string ApprovedByUserId
) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
