using MediatR;
using MyShippingPlatform.Application.Common.Interfaces;
using MyShippingPlatform.Application.Common.Models;
using MyShippingPlatform.Application.Shipments.DTOs;

namespace MyShippingPlatform.Application.Shipments.Commands.AuditShipment;

public record AuditShipmentCommand(Guid ShipmentId) : IRequest<Result<AuditShipmentResponseDto>>;

public class AuditShipmentCommandHandler : IRequestHandler<AuditShipmentCommand, Result<AuditShipmentResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAiAgentService _aiAgentService;

    public AuditShipmentCommandHandler(
        IApplicationDbContext context,
        IAiAgentService aiAgentService)
    {
        _context = context;
        _aiAgentService = aiAgentService;
    }

    public async Task<Result<AuditShipmentResponseDto>> Handle(AuditShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _context.GetShipmentWithAuditRecordsAsync(request.ShipmentId, cancellationToken);

        if (shipment == null)
        {
            return Result.Failure<AuditShipmentResponseDto>($"Shipment with ID {request.ShipmentId} not found.");
        }

        try
        {
            var aiResponse = await _aiAgentService.AuditShipmentAsync(
                shipperName: shipment.ShipperName,
                consigneeName: shipment.ConsigneeName,
                originCountry: shipment.OriginCountry,
                destinationCountry: shipment.DestinationCountry,
                cancellationToken: cancellationToken
            );

            string auditReason;

            if (aiResponse == null)
            {
                auditReason = "AI service unavailable or returned invalid response.";
                shipment.FlagForReview(auditReason);
            }
            else
            {
                auditReason = aiResponse.Reason;
                bool isApproved = aiResponse.Result.Equals("yes", StringComparison.OrdinalIgnoreCase);

                if (isApproved)
                {
                    shipment.SystemApprove(auditReason);
                }
                else
                {
                    shipment.FlagForReview(auditReason);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            var responseDto = new AuditShipmentResponseDto(
                ShipmentId: shipment.Id,
                Status: shipment.Status,
                StatusName: shipment.Status.ToString(),
                AiReason: auditReason,
                AuditedAt: DateTime.UtcNow
            );

            return Result.Success(responseDto);
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException || ex is InvalidOperationException)
        {
            return Result.Failure<AuditShipmentResponseDto>(ex.Message);
        }
    }
}