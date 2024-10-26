using API_Clients.Models;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using RabbitMQ.Client;
using Serilog;
using API_Clients.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration de Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


builder.Host.UseSerilog();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// Configuration de RabbitMQ
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory() { HostName = "localhost" };
    IConnection connection = null;
    var logger = sp.GetRequiredService<ILogger<IConnection>>();

    try
    {
        connection = factory.CreateConnection();

        if (connection.IsOpen)
        {
            logger.LogInformation("RabbitMQ connection created successfully and is open.");
        }
        else
        {
            logger.LogWarning("RabbitMQ connection was created but is not open.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to create RabbitMQ connection.");
        throw;
    }

    return connection;
});

builder.Services.AddSingleton<CustomerCheckConsumer>();
builder.Services.AddHostedService<RabbitMQHostedService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

// Utiliser le middleware Prometheus
app.UseMetricServer();  // Ajoute un endpoint pour les métriques Prometheus
app.UseHttpMetrics();   // Collecte les métriques HTTP (requêtes, latence, etc.)

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
