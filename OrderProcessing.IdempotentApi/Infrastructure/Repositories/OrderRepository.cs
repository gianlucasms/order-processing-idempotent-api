using Microsoft.EntityFrameworkCore;
using OrderProcessing.IdempotentApi.Domain.Entities;
using OrderProcessing.IdempotentApi.Domain.Interfaces;
using OrderProcessing.IdempotentApi.Infrastructure.Persistence;

namespace OrderProcessing.IdempotentApi.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.FindAsync([id], cancellationToken);
    }

    public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }
}
