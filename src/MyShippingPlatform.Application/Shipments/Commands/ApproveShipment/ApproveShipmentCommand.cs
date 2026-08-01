using MediatR;
using MyShippingPlatform.Application.Common.Interfaces;
using MyShippingPlatform.Application.Common.Models;

namespace MyShippingPlatform.Application.Shipments.Commands.ApproveShipment;

public record ApproveShipmentCommand(Guid ShipmentId) : IRequest<Result>;

public class ApproveShipmentCommandHandler : IRequestHandler<ApproveShipmentCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ApproveShipmentCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(ApproveShipmentCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure("User is not authenticated.");
        }

        // Clean call with zero EF Core extensions required in Application layer
        var shipment = await _context.GetShipmentWithAuditRecordsAsync(request.ShipmentId, cancellationToken);

        if (shipment == null)
        {
            return Result.Failure($"Shipment with ID {request.ShipmentId} not found.");
        }

        try
        {
            shipment.HumanApprove(userId, _currentUserService.Permissions);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException || ex is InvalidOperationException)
        {
            return Result.Failure(ex.Message);
        }
    }
}