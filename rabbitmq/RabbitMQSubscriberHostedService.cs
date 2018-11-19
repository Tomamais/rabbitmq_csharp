using messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using rabbitmq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace generichost
{
    public class RabbitMQSubscriberHostedService<TProcessor, TMessage> : IHostedService 
        where TProcessor: IProcessor<TMessage>
        where TMessage: IGenericMessage 
    {
        private readonly ILogger _logger;
        private readonly IApplicationLifetime _appLifetime;
        private readonly IProcessor<TMessage> _processor;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMQConfiguration _configuration;
        private readonly ConnectionFactory _factory;

        public RabbitMQSubscriberHostedService(
            ILogger<RabbitMQSubscriberHostedService<TProcessor, TMessage>> logger, 
            IApplicationLifetime appLifetime,
            IOptions<RabbitMQConfiguration> configuration,
            IProcessor<TMessage> processor)
        {
            _factory = new ConnectionFactory();
            _logger = logger;
            _appLifetime = appLifetime;
            _processor = processor;
            
            _configuration = configuration.Value;
            _factory.UserName = _configuration.UserName;
            _factory.Password = _configuration.Password;
            _factory.VirtualHost = _configuration.VirtualHost;
            _factory.HostName = _configuration.HostName;
            _factory.Port = _configuration.Port;

            // initialize RabbitMQ connection
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");

            // declare and create the queue
            string queueName = _configuration.SubscribedQueue.QueueName;
            string routingKey = queueName;
            _channel.QueueDeclare(queue: queueName,
                                durable: _configuration.SubscribedQueue.Durable,
                                exclusive: _configuration.SubscribedQueue.Exclusive,
                                autoDelete: _configuration.SubscribedQueue.AutoDelete,
                                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                TMessage message = default;
                try
                {
                    var body = ea.Body;
                    message = JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(body));
                    _processor.Process(message);
                    _logger.LogInformation(" [x] Received {0}", message);
                }
                catch (SerializationException ex)
                {
                    _logger.LogError(ex, "Message is on the wrong format");
                }
                catch (Exception ex)
                {
                    _processor.OnError(message, ex);
                }
            };

            String consumerTag = _channel.BasicConsume(queueName, true, consumer);
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");

            _connection.Close();
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");

            // Perform post-stopped activities here
        }
    }
}
