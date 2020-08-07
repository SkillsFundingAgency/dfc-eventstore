using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Telemetry.Models;
using DFC.Eventstore.Data.Models;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;
using System;

namespace DFC.App.EventStore.Data.Models
{
    public class EventStoreModel : RequestTrace, IDocumentModel
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "topic")]
        public string? Topic { get; set; }
        [JsonProperty(PropertyName = "subject")]
        public string? Subject { get; set; }
        [JsonProperty(PropertyName = "data")]
        public object? Data { get; set; }
        [JsonProperty(PropertyName = "eventType")]
        public string? EventType { get; set; }
        [JsonProperty(PropertyName = "eventTime")]
        public DateTime? EventTime { get; set; }
        [JsonProperty(PropertyName = "metadataVersion")]
        public string? MetadataVersion { get; }
        [JsonProperty(PropertyName = "dataVersion")]
        public string? DataVersion { get; set; }
        public string? Etag { get; set; }
        public string? PartitionKey { get; set; }
    }
}
