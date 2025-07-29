using studyBuddy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace studyBuddy.Core.Services
{
    public class LocalEmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _httpClient;

        public LocalEmbeddingService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8000/") };
        }

        public async Task<List<float>> GetEmbeddingAsync(string text)
        {
            var request = new EmbeddingRequest { texts = new List<string> { text } };
            var response = await _httpClient.PostAsJsonAsync("embed", request);
            var result = await response.Content.ReadFromJsonAsync<EmbeddingResponse>();

            if (result?.Embeddings == null || result.Embeddings.Count == 0)
                throw new InvalidOperationException("No embeddings returned from the service.");

            return result.Embeddings.First();
        }
    }
}
