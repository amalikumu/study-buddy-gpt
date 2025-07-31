using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public string Response { get; set; } = string.Empty;
    }
}
