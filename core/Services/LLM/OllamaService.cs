using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace studyBuddy.Core.Services.LLM
{
    public class OllamaService: ILlmService
    {
        private readonly HttpClient _httpClient;

        public OllamaService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:11434");
        }

        public async Task<string> GetAnswerAsync(string context, string question)
        {
            var prompt = $"Context:\n{context}\n\nQuestion: {question}";

            var request = new OllamaRequest
            {
                Model = "mistral",
                Prompt = prompt,
                Stream = false
            };

            var response = await _httpClient.PostAsJsonAsync("/api/generate", request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ollama returned {response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
            }

            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
            return result?.Response ?? "No answer generated.";
        }
    }
}
