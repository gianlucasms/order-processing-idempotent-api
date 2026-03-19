using Microsoft.EntityFrameworkCore;
using OrderProcessing.IdempotentApi.Application.Interfaces;
using OrderProcessing.IdempotentApi.Application.Services;
using OrderProcessing.IdempotentApi.Domain.Interfaces;
using OrderProcessing.IdempotentApi.Infrastructure.Persistence;
using OrderProcessing.IdempotentApi.Infrastructure.Repositories;
using OrderProcessing.IdempotentApi.Infrastructure.Services;
using OrderProcessing.IdempotentApi.Filters;
using OrderProcessing.IdempotentApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<IdempotencyHeaderOperationFilter>();
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=orders.db"));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IIdempotencyService, IdempotencyService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<IdempotencyMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
