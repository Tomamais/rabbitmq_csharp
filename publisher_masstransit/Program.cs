using System;
using generichost;
using MassTransit;

namespace publisher_masstransit
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(host: "localhost", port: 5672, virtualHost: "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            bus.Start();
            bus.Publish<MessageModel>(new MessageModel() { Identifier = Guid.NewGuid() });
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            bus.Stop();
        }
    }
}
