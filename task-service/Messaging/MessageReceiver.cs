using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using task_service.DBContexts;
using task_service.Services;
using task_service.Services.Interfaces;

namespace task_service.Messaging;

public class MessageReceiver : BackgroundService
{
    private IConnection _connection;
    private IModel _model;
    private const string _queue = "list_delete";
    private readonly ITaskService _taskService;
    
    public MessageReceiver(IServiceProvider serviceProvider)
    {
        _taskService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ITaskService>();
       Initialize();
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        
        var consumer = new EventingBasicConsumer(_model);
        consumer.Received += (sender, eventArgs) =>
        {
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            Console.WriteLine($"Received message: {message} in queue {_queue}");
            _taskService.DeleteByListId(Int32.Parse(message));
        };
        
        _model.BasicConsume(queue: _queue, autoAck: true, consumer: consumer);
        
        return Task.CompletedTask;
    }
    
    private void Initialize()
    {
        var factory = new ConnectionFactory
        {
            HostName = "38.242.252.134",
        };
        
        _connection = factory.CreateConnection();
        _model = _connection.CreateModel();
        _model.QueueDeclare(queue: _queue,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        
        Console.WriteLine("Connection initialized");
    }
}