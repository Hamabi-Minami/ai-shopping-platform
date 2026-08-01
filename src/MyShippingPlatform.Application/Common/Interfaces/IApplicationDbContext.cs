using System;
using System.Threading;
using System.Threading.Tasks;
using MyShippingPlatform.Domain.Entities;

namespace MyShippingPlatform.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    IQueryable<Shipment> Shipments { get; }
    IQueryable<AIAuditRecord> AIAuditRecords { get; }

    // Add a specific method to fetch aggregate root with navigation properties
    Task<Shipment?> GetShipmentWithAuditRecordsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}