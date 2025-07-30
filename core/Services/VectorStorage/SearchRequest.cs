using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace studyBuddy.Core.Services.VectorStorage
{
    public class SearchRequest
    {
        [JsonPropertyName("vector")]
        public float[] Vector { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("with_payload")]
        public bool WithPayload { get; set; } = true;
    }

    public class SearchResponse
    {
        [JsonPropertyName("result")]
        public List<QdrantPoint> Result { get; set; }
    }

    public class QdrantPoint
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("payload")]
        public Dictionary<string, object> Payload { get; set; }

        [JsonPropertyName("score")]
        public float Score { get; set; }
    }
}
