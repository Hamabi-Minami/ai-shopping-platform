using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShippingPlatform.Domain.Enums;

public enum ShipmentStatus
{
    Draft = 1,          // Initial state before submission
    Auditing = 2,       // Document is being analyzed by AI Agent
    Flagged = 3,        // AI detected potential risks, pending human review
    AiApproved = 4,     // System automated approval by AI Agent
    HumanApproved = 5,  // Manual override/approval by human compliance reviewer
    Dispatched = 6,     // Shipment handed over to carrier
    InTransit = 7,      // En route to destination
    Delivered = 8       // Successfully delivered
}
