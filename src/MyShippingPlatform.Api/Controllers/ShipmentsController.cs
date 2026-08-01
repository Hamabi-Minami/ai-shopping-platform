using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyShippingPlatform.Application.Shipments.Commands.ApproveShipment;

namespace MyShippingPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly ISender _mediator;

    public ShipmentsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var command = new ApproveShipmentCommand(id);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Error = result.ErrorMessage });
        }

        return Ok(new { Message = "Shipment approved successfully." });
    }
}