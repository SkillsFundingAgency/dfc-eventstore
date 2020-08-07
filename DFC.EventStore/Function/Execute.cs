using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DFC.App.EventStore.Data.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using DFC.Compui.Cosmos.Contracts;

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
           ([EventGridTrigger] EventStoreModel eventGridEvent, ILogger log)
        {
            try
            {
                if (eventGridEvent != null)
                {
                    log.LogInformation($"Request received: {eventGridEvent}");

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
