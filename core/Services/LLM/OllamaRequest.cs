using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace studyBuddy.Core.Services.LLM
{
    public class OllamaRequest
    {
        public string Model { get; set; } = "mistral";
        public string Prompt { get; set; } = string.Empty;
        public bool Stream { get; set; } = false;
    }

    public class OllamaResponse
    {
        [JsonPropertyName("response")]
        public string Response { get; set; } = string.Empty;
    }
}
