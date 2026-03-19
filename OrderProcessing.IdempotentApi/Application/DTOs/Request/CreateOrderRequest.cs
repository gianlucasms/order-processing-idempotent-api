using System.ComponentModel.DataAnnotations;

namespace OrderProcessing.IdempotentApi.DTOs.Request;

public record CreateOrderRequest(
    [Required, MinLength(2)] string CustomerName,
    [Required, MinLength(2)] string ProductName,
    [Range(1, int.MaxValue)] int Quantity,
    [Range(0.01, double.MaxValue)] decimal UnitPrice
);
