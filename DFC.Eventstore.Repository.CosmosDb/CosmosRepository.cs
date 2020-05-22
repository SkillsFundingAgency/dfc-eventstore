using DFC.Eventstore.Data.Contracts;
using DFC.Eventstore.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Eventstore.Repository.CosmosDb
{
    [ExcludeFromCodeCoverage]
    public class CosmosRepository<T> : ICosmosRepository<T>
        where T : class, IDataModel
    {
        private readonly CosmosDbConnection cosmosDbConnection;
        private readonly IDocumentClient documentClient;
        private readonly IHostingEnvironment env;

        public CosmosRepository(CosmosDbConnection cosmosDbConnection, IDocumentClient documentClient, IHostingEnvironment env)
        {
            this.cosmosDbConnection = cosmosDbConnection;
            this.documentClient = documentClient;
            this.env = env;
            InitialiseDevEnvironment().GetAwaiter().GetResult();
        }

        private Uri DocumentCollectionUri => UriFactory.CreateDocumentCollectionUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId);

        public async Task InitialiseDevEnvironment()
        {
            if (env.IsDevelopment())
            {
                await CreateDatabaseIfNotExistsAsync().ConfigureAwait(false);
                await CreateCollectionIfNotExistsAsync().ConfigureAwait(false);
            }
        }

        public async Task<HttpStatusCode> CreateAsync(T model)
        {
            if (model != null)
            {
                var result = await documentClient.CreateDocumentAsync(DocumentCollectionUri, model, new RequestOptions { PartitionKey = new PartitionKey(model.EventType) }).ConfigureAwait(false);

                return result.StatusCode;
            }

            return HttpStatusCode.BadRequest;
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(cosmosDbConnection.DatabaseId)).ConfigureAwait(false);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await documentClient.CreateDatabaseAsync(new Database { Id = cosmosDbConnection.DatabaseId }).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId)).ConfigureAwait(false);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    var partitionKeyDefinition = new PartitionKeyDefinition
                    {
                        Paths = new Collection<string>() { "/eventType" },
                    };

                    await documentClient.CreateDocumentCollectionAsync(
                                UriFactory.CreateDatabaseUri(cosmosDbConnection.DatabaseId),
                                new DocumentCollection { Id = cosmosDbConnection.CollectionId, PartitionKey = partitionKeyDefinition },
                                new RequestOptions { OfferThroughput = 1000 }).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
