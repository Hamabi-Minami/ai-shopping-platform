using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MyShippingPlatform.Domain.Common;

/// <summary>
/// Pure Domain Event Marker Interface (Zero External Dependencies)
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}