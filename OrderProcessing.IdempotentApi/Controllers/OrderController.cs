using Microsoft.AspNetCore.Mvc;
using OrderProcessing.IdempotentApi.Application.Interfaces;
using OrderProcessing.IdempotentApi.DTOs.Request;
using OrderProcessing.IdempotentApi.DTOs.Response;

namespace OrderProcessing.IdempotentApi.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received order creation request for customer {CustomerName}", request.CustomerName);

        var response = await _orderService.CreateOrderAsync(request, cancellationToken);

        return CreatedAtAction(nameof(CreateOrder), new { id = response.Id }, response);
    }
}
