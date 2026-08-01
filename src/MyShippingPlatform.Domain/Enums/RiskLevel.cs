using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShippingPlatform.Domain.Enums;

public enum RiskLevel
{
    Low = 1,      // Normal / Low risk
    Medium = 2,   // Anomalies detected / Needs attention
    High = 3      // Severe discrepancy / Customs risk flagged
}
