using Microsoft.Extensions.Configuration;
using studyBuddy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace studyBuddy.Core.Services
{
    public class OpenAIEmbeddingService: IEmbeddingService
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly HttpClient _httpClient;

        public OpenAIEmbeddingService(IConfiguration config)
        {
            _apiKey = config["OpenAI:ApiKey"]!;
            _model = config["OpenAI:EmbeddingModel"] ?? "text-embedding-3-small";

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<List<float>> GetEmbeddingAsync(string input)
        {
            var requestBody = new
            {
                input = input,
                model = _model
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/embeddings", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API Error: {response.StatusCode} - {errorContent}", null, response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EmbeddingResponse>(json);

            return result?.Embeddings?.FirstOrDefault() ?? new List<float>();
        }

        /// <summary>
        /// The embedding with retry logic to handle rate limits and service unavailability as exponential backoff.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="maxRetries"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<float>> GetEmbeddingWithRetryAsync(string input, int maxRetries = 5)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    return await GetEmbeddingAsync(input);
                }
                catch (HttpRequestException ex) when (
                    ex.StatusCode == HttpStatusCode.TooManyRequests || ex.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    int delay = attempt * 2000; // exponential backoff (ms)
                    Console.WriteLine($"⚠️ Rate limit hit or service unavailable. Attempt {attempt}/{maxRetries}. Retrying in {delay / 1000} sec...");
                    await Task.Delay(delay);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Unexpected error: {ex.Message}");
                    throw;
                }
            }

            throw new Exception("❌ Failed to get embedding after multiple retries.");
        }
    }
}
