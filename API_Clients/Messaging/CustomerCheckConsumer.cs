using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using API_Clients.Models;
using API_Clients.Services;
using Microsoft.Extensions.DependencyInjection;
public class CustomerCheckConsumer : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<CustomerCheckConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CustomerCheckConsumer(IConnection connection, ILogger<CustomerCheckConsumer> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        try
        {
            _connection = connection;
            _channel = _connection.CreateModel();
            _logger.LogInformation("Connected to RabbitMQ for customer check.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to initialize CustomerCheck consumer: {ex.Message}");
        }
    }

    public void StartConsuming()
    {
        try
        {
            _channel.QueueDeclare(queue: "customer_check_request",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            _logger.LogInformation("Queue declared: customer_check_request");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var customerCheckRequest = JsonSerializer.Deserialize<CustomerCheckRequest>(message);
                    _logger.LogInformation($"Customer check request received: {message}");

                    await ProcessCustomerCheckRequest(customerCheckRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing customer check request: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: "customer_check_request", autoAck: true, consumer: consumer);
            _logger.LogInformation("Customer check consumer started and attached to queue: customer_check_request");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to start consuming customer check requests: {ex.Message}");
        }
    }
    private async Task ProcessCustomerCheckRequest(CustomerCheckRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();

            bool isCustomerValid = await customerService.CheckCustomerStatus(request.ClientId);

            SendCustomerCheckResponse(request.ClientId, isCustomerValid);
        }
    }


    private void SendCustomerCheckResponse(int clientId, bool isCustomerValid)
    {
        var response = new CustomerCheckResponse
        {
            ClientId = clientId,
            IsCustomerValid = isCustomerValid
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        var body = Encoding.UTF8.GetBytes(jsonResponse);

        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        _channel.BasicPublish(exchange: "",
                               routingKey: "customer_check_response_queue",
                               basicProperties: properties,
                               body: body);

        _logger.LogInformation($"Sent customer check response for Client ID: {clientId}, IsCustomerValid: {isCustomerValid}");
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
