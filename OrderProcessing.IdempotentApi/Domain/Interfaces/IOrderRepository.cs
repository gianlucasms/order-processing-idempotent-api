using OrderProcessing.IdempotentApi.Domain.Entities;

namespace OrderProcessing.IdempotentApi.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);
}
