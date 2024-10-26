using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

public class RabbitMQHostedService : IHostedService
{
    private readonly CustomerCheckConsumer _customerCheckConsumer;
    public RabbitMQHostedService(CustomerCheckConsumer customerCheckConsumer)
    {
        _customerCheckConsumer = customerCheckConsumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _customerCheckConsumer.StartConsuming();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _customerCheckConsumer.Dispose();
        return Task.CompletedTask;
    }
}
