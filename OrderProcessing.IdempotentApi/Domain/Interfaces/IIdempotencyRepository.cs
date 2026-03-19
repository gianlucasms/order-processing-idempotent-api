using OrderProcessing.IdempotentApi.Domain.Entities;

namespace OrderProcessing.IdempotentApi.Domain.Interfaces;

public interface IIdempotencyRepository
{
    Task<IdempotencyRecord?> GetByKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default);
    Task AddAsync(IdempotencyRecord record, CancellationToken cancellationToken = default);
}
