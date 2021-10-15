using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NsbCosmosOutbox.Worker.Behaviors;
using NServiceBus;

namespace NsbCosmosOutbox.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .ConfigureServices((hostContext, services) =>
                // {
                //     services.AddHostedService<Worker>();
                // })
                .UseNServiceBus(_ =>
                {
                    var endpointConfiguration = new EndpointConfiguration("NsbCosmosOutbox.Worker");
                    endpointConfiguration.EnableOutbox();

                    var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
                    var connection = @"AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
                    persistence.DatabaseName("NsbCosmosOutbox.CosmosDB.Transactions");
                    persistence.CosmosClient(new CosmosClient(connection));
                    persistence.DefaultContainer("Todos", "/TodoId");
                    
                    endpointConfiguration.Pipeline.Register(new TodoIdAsPartitionKeyBehavior.Registration());
                    

                    endpointConfiguration.UseTransport<RabbitMQTransport>()
                        .UseConventionalRoutingTopology()
                        .ConnectionString("host=localhost;username=guest;password=guest;virtualHost=/;");
                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                });
    }
}