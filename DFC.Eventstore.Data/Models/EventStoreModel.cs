using DFC.Eventstore.Data.Models;
using Microsoft.Azure.EventGrid.Models;

namespace DFC.App.EventStore.Data.Models
{
    public class EventStoreModel : EventGridEvent, IDataModel
    {
    }
}
