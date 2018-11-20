using System;
using System.Collections.Generic;
using System.Text;

namespace rabbitmq
{
    public class RabbitMQConfiguration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public ushort Port { get; set; }
        public SubscribedQueue SubscribedQueue { get; set; }
        public PublisherQueue PublisherQueue { get; set; }
    }

    public class SubscribedQueue
    {
        public string QueueName { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
    }

    public class PublisherQueue
    {
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
    }
}
