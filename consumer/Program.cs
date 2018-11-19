using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory();

            factory.UserName = "guest";
            factory.Password = "guest";
            factory.VirtualHost = "/";
            factory.HostName = "localhost";
            factory.Port = 5672;

            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    // declare and create the queue
                    string queueName = "MyNewQueue";
                    string routingKey = queueName;
                    channel.QueueDeclare(queue: queueName,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (ch, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    };

                    String consumerTag = channel.BasicConsume(queueName, true, consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
