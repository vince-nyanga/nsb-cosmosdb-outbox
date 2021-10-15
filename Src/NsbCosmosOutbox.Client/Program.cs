using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NsbCosmosOutbox.Shared.Commands;
using NServiceBus;

namespace NsbCosmosOutbox.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseNServiceBus(_ =>
                {
                    var endpointConfiguration = new EndpointConfiguration("NsbCosmosOutbox.Client");
                    endpointConfiguration.SendOnly();
                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>()
                        .UseConventionalRoutingTopology()
                        .ConnectionString("host=localhost;username=guest;password=guest;virtualHost=/;");
                    
                    var routing = transport.Routing();
                    routing.RouteToEndpoint(assembly: typeof(AddTodo).Assembly,
                        destination: "NsbCosmosOutbox.Worker");
                    return endpointConfiguration;
                });
    }
}