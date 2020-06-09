using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DFC.Eventstore.Data.Contracts;
using DFC.App.EventStore.Data.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json;

namespace DFC.EventStore.Function
{
    public class Execute
    {
        private readonly ICosmosRepository<EventStoreModel> _eventstoreRepository;

        public Execute(ICosmosRepository<EventStoreModel> eventstoreRepository)
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
                    log.LogInformation($"Request received: {JsonConvert.SerializeObject(eventGridEvent)}");
                    await _eventstoreRepository.CreateAsync(eventGridEvent);
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
