namespace OrderProcessing.IdempotentApi.Domain.Entities;

public class IdempotencyRecord
{
    public Guid Id { get; private set; }
    public string IdempotencyKey { get; private set; }
    public int StatusCode { get; private set; }
    public string ResponsePayload { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private IdempotencyRecord()
    {
        IdempotencyKey = string.Empty;
        ResponsePayload = string.Empty;
    }

    public static IdempotencyRecord Create(string idempotencyKey, int statusCode, string responsePayload)
    {
        return new IdempotencyRecord
        {
            Id = Guid.NewGuid(),
            IdempotencyKey = idempotencyKey,
            StatusCode = statusCode,
            ResponsePayload = responsePayload,
            CreatedAt = DateTime.UtcNow
        };
    }
}
