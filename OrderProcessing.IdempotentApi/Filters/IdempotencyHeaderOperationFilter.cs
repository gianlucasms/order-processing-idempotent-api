using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OrderProcessing.IdempotentApi.Filters;

public class IdempotencyHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var method = context.ApiDescription.HttpMethod;

        if (!string.Equals(method, HttpMethods.Post, StringComparison.OrdinalIgnoreCase))
            return;

        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Idempotency-Key",
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema { Type = "string" },
            Description = "Unique key to ensure idempotent request processing."
        });
    }
}
