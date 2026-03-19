using OrderProcessing.IdempotentApi.Application.Interfaces;
using OrderProcessing.IdempotentApi.Domain.Entities;
using OrderProcessing.IdempotentApi.Domain.Interfaces;
using OrderProcessing.IdempotentApi.DTOs.Request;
using OrderProcessing.IdempotentApi.DTOs.Response;

namespace OrderProcessing.IdempotentApi.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = Order.Create(
            request.CustomerName,
            request.ProductName,
            request.Quantity,
            request.UnitPrice
        );

        await _orderRepository.AddAsync(order, cancellationToken);

        _logger.LogInformation("Order {OrderId} created for customer {CustomerName}", order.Id, order.CustomerName);

        return MapToResponse(order);
    }

    private static OrderResponse MapToResponse(Order order) =>
        new(
            order.Id,
            order.CustomerName,
            order.ProductName,
            order.Quantity,
            order.UnitPrice,
            order.TotalPrice,
            order.Status.ToString(),
            order.CreatedAt
        );
}
