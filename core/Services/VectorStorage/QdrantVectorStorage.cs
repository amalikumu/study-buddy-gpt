using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace studyBuddy.Core.Services.VectorStorage
{
    public class QdrantVectorStorage : IVectorStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _url;

        public QdrantVectorStorage(string baseUrl, string collectionName)
        {
            _url = $"/collections/{collectionName}";
            _baseUrl = baseUrl.TrimEnd('/');
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<bool> CreateCollectionAsync(int vectorSize)
        {
            var payload = new
            {
                vectors = new
                {
                    size = vectorSize,
                    distance = "Cosine"
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(_url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpsertPointsAsync(List<VectorPoint> points)
        {
            var url = $"{_url}/points";

            var pointList = points.Select(p => new
            {
                id = p.Id,
                vector = p.Vector,
                payload = p.Payload
            });

            var payload = new { points = pointList };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<string>> SearchAsync(float[] queryEmbedding, int topK = 5)
        {
            var url = $"{_url}/points/search";
            var searchRequest = new SearchRequest
            {
                Vector = queryEmbedding,
                Limit = topK//,
                //WithPayload = true
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var payloadJson = JsonSerializer.Serialize<SearchRequest>(searchRequest, options);

            var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Search failed: {response.StatusCode}\n{error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var searchResponse = JsonSerializer.Deserialize<SearchResponse>(json);

            return searchResponse.Result
                .Select(p => p.Payload["text"].ToString())
                .ToList();
        }
    }
}
