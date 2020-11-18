using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DFC.App.EventStore.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using System.Diagnostics;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;

namespace DFC.EventStore.Function
{
    public class Execute
    {
        private readonly IDocumentService<EventStoreModel> _eventstoreRepository;

        public Execute(IDocumentService<EventStoreModel> eventstoreRepository)
        {
            _eventstoreRepository = eventstoreRepository ?? throw new ArgumentNullException(nameof(eventstoreRepository));
        }

        [FunctionName("Execute")]
        public async Task Run
           ([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Execute")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();

                var eventGridEvent = JsonConvert.DeserializeObject<EventStoreModel>(content);

                if (eventGridEvent != null)
                {
                    if (Activity.Current == null)
                    {
                        Activity.Current = new Activity("EventStoreExecute").Start();
                    }

                    log.LogInformation($"Request received: {eventGridEvent}");

                    eventGridEvent.PartitionKey = eventGridEvent.EventType;
                    await _eventstoreRepository.UpsertAsync(eventGridEvent);
                }
            }

            catch (Exception e)
            {
                log.LogError(e, $"Error occured in {nameof(Execute)}");
                throw;
            }
        }
    }
}
