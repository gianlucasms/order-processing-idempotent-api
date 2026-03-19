using System.Text.Json;
using OrderProcessing.IdempotentApi.Application.Interfaces;
using OrderProcessing.IdempotentApi.Domain.Entities;
using OrderProcessing.IdempotentApi.Domain.Interfaces;

namespace OrderProcessing.IdempotentApi.Infrastructure.Services;

public class IdempotencyService : IIdempotencyService
{
    private readonly IIdempotencyRepository _idempotencyRepository;
    private readonly ILogger<IdempotencyService> _logger;

    public IdempotencyService(IIdempotencyRepository idempotencyRepository, ILogger<IdempotencyService> logger)
    {
        _idempotencyRepository = idempotencyRepository;
        _logger = logger;
    }

    public async Task<IdempotencyResult?> GetExistingResponseAsync(string idempotencyKey, CancellationToken cancellationToken = default)
    {
        var record = await _idempotencyRepository.GetByKeyAsync(idempotencyKey, cancellationToken);

        if (record is null)
            return null;

        _logger.LogInformation("Idempotency key {IdempotencyKey} found — returning cached response", idempotencyKey);

        return new IdempotencyResult(record.StatusCode, record.ResponsePayload);
    }

    public async Task StoreResponseAsync(string idempotencyKey, int statusCode, object responsePayload, CancellationToken cancellationToken = default)
    {
        var serialized = JsonSerializer.Serialize(responsePayload);
        var record = IdempotencyRecord.Create(idempotencyKey, statusCode, serialized);

        await _idempotencyRepository.AddAsync(record, cancellationToken);

        _logger.LogInformation("Stored idempotency record for key {IdempotencyKey}", idempotencyKey);
    }
}
