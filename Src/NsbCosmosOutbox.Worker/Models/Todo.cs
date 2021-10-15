using System;
using Newtonsoft.Json;

namespace NsbCosmosOutbox.Worker.Models
{
    public class Todo
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        public Guid TodoId { get; set; }
        public string Task { get; set; }
    }
}