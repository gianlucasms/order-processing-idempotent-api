using Microsoft.EntityFrameworkCore;
using OrderProcessing.IdempotentApi.Domain.Entities;
using OrderProcessing.IdempotentApi.Domain.Interfaces;
using OrderProcessing.IdempotentApi.Infrastructure.Persistence;

namespace OrderProcessing.IdempotentApi.Infrastructure.Repositories;

public class IdempotencyRepository : IIdempotencyRepository
{
    private readonly AppDbContext _context;

    public IdempotencyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IdempotencyRecord?> GetByKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default)
    {
        return await _context.IdempotencyRecords
            .FirstOrDefaultAsync(r => r.IdempotencyKey == idempotencyKey, cancellationToken);
    }

    public async Task AddAsync(IdempotencyRecord record, CancellationToken cancellationToken = default)
    {
        await _context.IdempotencyRecords.AddAsync(record, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
