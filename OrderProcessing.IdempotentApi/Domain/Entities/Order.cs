namespace OrderProcessing.IdempotentApi.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public string CustomerName { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice => Quantity * UnitPrice;
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }

    private Order()
    {
        CustomerName = string.Empty;
        ProductName = string.Empty;
    }

    public static Order Create(string customerName, string productName, int quantity, decimal unitPrice)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = customerName,
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending
        };
    }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Cancelled
}
