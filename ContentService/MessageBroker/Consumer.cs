using ContentService.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ContentService.MessageBroker;

using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class Consumer : IHostedService
{
    private const string QueueName = "response_model";
    private readonly IServiceProvider _serviceProvider;
    private IConnection _connection;
    private IChannel _channel;
    
    public Consumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Received message: {message}");

            // Process
            
            // using var scope = _serviceProvider.CreateScope();
            // var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(queue: QueueName, autoAck: true, consumer: consumer);

    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}