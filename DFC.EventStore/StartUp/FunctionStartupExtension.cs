using DFC.App.EventStore.Data.Models;
using DFC.Eventstore.Data.Contracts;
using DFC.Eventstore.Repository.CosmosDb;
using DFC.ServiceTaxonomy.ApiFunction.StartUp;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

[assembly: FunctionsStartup(typeof(FunctionStartupExtension))]

namespace DFC.ServiceTaxonomy.ApiFunction.StartUp
{
    [ExcludeFromCodeCoverage]
    public class FunctionStartupExtension : FunctionsStartup
    {
        public const string CosmosDbConfigAppSettings = "Configuration:CosmosDbConnections:Eventstore";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.ToLowerInvariant()}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var cosmosDbConnection = config.GetSection(CosmosDbConfigAppSettings).Get<CosmosDbConnection>();
            var documentClient = new DocumentClient(cosmosDbConnection!.EndpointUrl, cosmosDbConnection!.AccessKey);


            builder.Services.AddTransient<ICosmosRepository<EventStoreModel>, CosmosRepository<EventStoreModel>>();

            builder.Services.AddSingleton(cosmosDbConnection);
            builder.Services.AddSingleton<IDocumentClient>(documentClient);

            builder.Services.AddSingleton<IConfiguration>(config);
        }
    }
}
