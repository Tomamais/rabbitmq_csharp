using System;
using System.IO;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using rabbitmq;

namespace generichost
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configApp.AddEnvironmentVariables(prefix: "PREFIX_");
                    configApp.AddCommandLine(args);
                })
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddSingleton<IBusControl>(serviceProvider =>
                     {
                         return Bus.Factory.CreateUsingRabbitMq(cfg =>
                         {
                             var host = cfg.Host(host: "localhost", port: 5672, virtualHost: "/", h =>
                             {
                                 h.Username("guest");
                                 h.Password("guest");
                             });
                             
                             cfg.ReceiveEndpoint(
                                 host: host, 
                                 queueName: "MyNewQueue", 
                                 e => {
                                     //  e.Observer<ConsumerObserver>();
                                     e.Consumer<Consumer>();
                                     e.Durable = true;
                                 });
                         });
                     });
                     services.AddScoped<IHostedService, MassTransitBusControlService>();
                 })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .UseConsoleLifetime()
                .Build();

            await hostBuilder.RunAsync();
        }
    }
}
