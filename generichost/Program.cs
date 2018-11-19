using System;
using System.IO;
using System.Threading.Tasks;
using domain;
using messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using rabbitmq;
using RabbitMQ.Client;

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
                    // RabbitMQ configuration
                    services.Configure<RabbitMQConfiguration>(hostContext.Configuration.GetSection("RabbitMQConfiguration"));
                    // if the serializer is needed
                    // services.AddSingleton<RabbitMQSerializer<GenericMessage>>();
                    services.AddScoped<IProcessor<GenericMessage>, GenericProcessor>();
                    services.AddHostedService<RabbitMQSubscriberHostedService<GenericProcessor, GenericMessage>>();
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
