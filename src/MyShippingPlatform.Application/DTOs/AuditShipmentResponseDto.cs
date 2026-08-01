using MyShippingPlatform.Domain.Enums;

namespace MyShippingPlatform.Application.Shipments.DTOs;

public record AuditShipmentResponseDto(
    Guid ShipmentId,
    ShipmentStatus Status,
    string StatusName,
    string AiReason,
    DateTime AuditedAt
);