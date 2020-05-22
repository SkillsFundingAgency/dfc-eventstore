namespace DFC.Eventstore.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public interface IDataModel
    {
        [Required]
        [JsonProperty(PropertyName = "id")]
        string? Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "eventType")]
        string? EventType { get; set; }
    }
}
