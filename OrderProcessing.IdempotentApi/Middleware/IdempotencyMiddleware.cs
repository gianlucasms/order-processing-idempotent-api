using System.Text.Json;
using OrderProcessing.IdempotentApi.Application.Interfaces;

namespace OrderProcessing.IdempotentApi.Middleware;

public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IdempotencyMiddleware> _logger;

    public IdempotencyMiddleware(RequestDelegate next, ILogger<IdempotencyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IIdempotencyService idempotencyService)
    {
        if (!HttpMethods.IsPost(context.Request.Method))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey) || string.IsNullOrWhiteSpace(idempotencyKey))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            var error = JsonSerializer.Serialize(new { error = "The 'Idempotency-Key' header is required." });
            await context.Response.WriteAsync(error);
            return;
        }

        var existing = await idempotencyService.GetExistingResponseAsync(idempotencyKey!, context.RequestAborted);

        if (existing is not null)
        {
            _logger.LogInformation("Replaying cached response for Idempotency-Key: {IdempotencyKey}", idempotencyKey.ToString());
            context.Response.StatusCode = existing.StatusCode;
            context.Response.ContentType = "application/json";
            context.Response.Headers["X-Idempotency-Replayed"] = "true";
            await context.Response.WriteAsync(existing.Payload);
            return;
        }

        var originalBody = context.Response.Body;
        using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        await _next(context);

        buffer.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(buffer).ReadToEndAsync();

        if (context.Response.StatusCode is >= 200 and < 300)
        {
            await idempotencyService.StoreResponseAsync(
                idempotencyKey!,
                context.Response.StatusCode,
                JsonSerializer.Deserialize<object>(responseBody)!,
                context.RequestAborted
            );
        }

        buffer.Seek(0, SeekOrigin.Begin);
        await buffer.CopyToAsync(originalBody);
        context.Response.Body = originalBody;
    }
}
