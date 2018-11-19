using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using domain;
using generichost;
using messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using rabbitmq;
using Swashbuckle.AspNetCore.Swagger;

namespace aspnetcore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
            .AddAuthorization()
            .AddJsonFormatters()
            .AddApiExplorer()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RabbitMQConfiguration>(Configuration.GetSection("RabbitMQConfiguration"));
            // if the serializer is needed
            // services.AddSingleton<RabbitMQSerializer<GenericMessage>>();
            services.AddScoped<IMessagePublisher<GenericMessage>, RabbitMQPublisher<GenericMessage>>();

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "RabbitMQ Publisher",
                    Version = "v1",
                    Description = "The RabbitMQ Publisher HTTP API.",
                    TermsOfService = "Terms Of Service"
                });
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {
                        "Bearer", Enumerable.Empty<string>()
                    },
                });
            });

            //services.AddSingleton<RabbitMQListener>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseRabbitListener();
            app.UseMvc();
            app.UseSwagger()
           .UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
           });
        }
    }
}
