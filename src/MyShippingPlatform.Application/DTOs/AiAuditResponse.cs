namespace MyShippingPlatform.Application.DTOs;

public class AiAuditResponse
{
    // Strict binary result: "yes" or "no"
    public string Result { get; set; } = string.Empty;

    // A short 1-sentence explanation to be logged into AIAuditRecords
    public string Reason { get; set; } = string.Empty;
}