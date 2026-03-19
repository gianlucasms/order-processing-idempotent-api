using OrderProcessing.IdempotentApi.DTOs.Request;
using OrderProcessing.IdempotentApi.DTOs.Response;

namespace OrderProcessing.IdempotentApi.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
}
