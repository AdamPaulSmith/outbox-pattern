using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OutboxPattern.Customers.Tests.Integration.ResponseModels
{
    internal class Http400StatusResponseModel
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, string[]> Errors { get; set; }
    }
}