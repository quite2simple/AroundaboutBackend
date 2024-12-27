using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

// Rabbit MQ

var factory = new ConnectionFactory { HostName = "localhost" };
var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();
var sendChannel = await connection.CreateChannelAsync();

const string queueName = "call_model";
const string sendQueue = "response_model";

await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false,
    arguments: null);
await sendChannel.QueueDeclareAsync(queue: sendQueue, durable: false, exclusive: false, autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    // responding back
    var responseMessage = (message.Length % 2 == 0) ? "true" : "false";
    var sendBody = Encoding.UTF8.GetBytes(responseMessage);
    await sendChannel.BasicPublishAsync(exchange: string.Empty, routingKey: sendQueue, body: sendBody);
    Console.WriteLine(" [x] Sent {responseMessage}");
    // return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);

// ASP.NET Core

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();