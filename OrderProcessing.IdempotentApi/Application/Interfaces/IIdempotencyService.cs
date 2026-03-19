using OrderProcessing.IdempotentApi.DTOs.Response;

namespace OrderProcessing.IdempotentApi.Application.Interfaces;

public interface IIdempotencyService
{
    Task<IdempotencyResult?> GetExistingResponseAsync(string idempotencyKey, CancellationToken cancellationToken = default);
    Task StoreResponseAsync(string idempotencyKey, int statusCode, object responsePayload, CancellationToken cancellationToken = default);
}

public record IdempotencyResult(int StatusCode, string Payload);
