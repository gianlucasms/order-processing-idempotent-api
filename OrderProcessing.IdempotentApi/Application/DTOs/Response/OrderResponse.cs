namespace OrderProcessing.IdempotentApi.DTOs.Response;

public record OrderResponse(
    Guid Id,
    string CustomerName,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt
);
