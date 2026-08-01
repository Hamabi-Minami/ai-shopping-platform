using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShippingPlatform.Application.Common.Interfaces;
using MyShippingPlatform.Domain.Entities;

namespace MyShippingPlatform.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    // Required: Pass options into base DbContext
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<AIAuditRecord> AIAuditRecords => Set<AIAuditRecord>();

    IQueryable<Shipment> IApplicationDbContext.Shipments => Shipments;
    IQueryable<AIAuditRecord> IApplicationDbContext.AIAuditRecords => AIAuditRecords;

    public async Task<Shipment?> GetShipmentWithAuditRecordsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Shipments
            .Include(s => s.AuditRecords)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public void Add<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
    }

    public void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
    }

    public void Remove<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}