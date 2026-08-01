using MyShippingPlatform.Application.DTOs;


namespace MyShippingPlatform.Application.Common.Interfaces;

public interface IAiAgentService
{
    Task<AiAuditResponse?> AuditShipmentAsync(
        string shipperName,
        string consigneeName,
        string originCountry,
        string destinationCountry,
        CancellationToken cancellationToken = default);
}