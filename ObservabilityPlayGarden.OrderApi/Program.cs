using ObservabilityPlayGarden.OpenTelemetry.Shared;
using ObservabilityPlayGarden.OrderApi.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetryExt(builder.Configuration);

builder.Services.AddScoped<OrderService>(); // request response kadar kullanayým sonra nesne dispose olsun. business logic scoped olmalý çünkü dbContext scope. (transaction saðlama amacýyla)

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOpenTelemetryPrometheusScrapingEndpoint();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
