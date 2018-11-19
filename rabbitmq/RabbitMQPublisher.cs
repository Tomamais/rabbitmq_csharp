using messaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace rabbitmq
{
    public class RabbitMQPublisher<TMessage> : IMessagePublisher<TMessage> where TMessage : IGenericMessage
    {
        private readonly RabbitMQConfiguration _configuration;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;

        public RabbitMQPublisher(
            IOptions<RabbitMQConfiguration> configuration)
        {
            _factory = new ConnectionFactory();

            _configuration = configuration.Value;
            _factory.UserName = _configuration.UserName;
            _factory.Password = _configuration.Password;
            _factory.VirtualHost = _configuration.VirtualHost;
            _factory.HostName = _configuration.HostName;
            _factory.Port = _configuration.Port;

            // initialize RabbitMQ connection
            _connection = _factory.CreateConnection();
        }

        public void SendMessage(TMessage message)
        {
            using (var channel = _connection.CreateModel())
            {
                var messageJson = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageJson);

                channel.BasicPublish(exchange: _configuration.PublishingQueue.ExchangeName,
                                     routingKey: _configuration.PublishingQueue.RoutingKey,
                                     basicProperties: null,
                                     body: body);

            }
        }
    }
}
