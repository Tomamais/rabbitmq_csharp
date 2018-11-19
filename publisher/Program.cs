using System;
using System.Text;
using RabbitMQ.Client;

namespace publisher
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
                    string exchangeName = "";
                    string queueName = "MyNewQueue";
                    string routingKey = queueName;

                    string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: queueName,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
