using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace rabbitmq
{
    public class RabbitMQListener
    {
        ConnectionFactory Factory { get; set; }
        IConnection Connection { get; set; }
        IModel Channel { get; set; }

        public void Register()
        {
            Channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
            };
            Channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
        }

        public void Deregister()
        {
            this.Connection.Close();
        }

        public RabbitMQListener()
        {
            this.Factory = new ConnectionFactory() { HostName = "localhost" };
            this.Connection = Factory.CreateConnection();
            this.Channel = Connection.CreateModel();
        }
    }
}
