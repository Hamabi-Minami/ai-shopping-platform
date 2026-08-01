using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyShippingPlatform.Application.Shipments.Commands.AuditShipment;

namespace MyShippingPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestAgentController : ControllerBase
{
    private readonly ISender _mediator;

    public TestAgentController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("test-audit/{shipmentId:guid}")]
    public async Task<IActionResult> TestAudit(Guid shipmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AuditShipmentCommand(shipmentId), cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new
            {
                Success = false,
                ShipmentId = shipmentId,
                ErrorMessage = result.ErrorMessage
            });
        }

        return Ok(new
        {
            Success = true,
            ShipmentId = result.Data?.ShipmentId,
            CurrentStatus = result.Data?.StatusName,
            StatusCode = (int?)result.Data?.Status,
            Reason = result.Data?.AiReason,
            AuditedAt = result.Data?.AuditedAt
        });
    }
}