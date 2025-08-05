namespace studyBuddy.Core.Services.LLM
{
    public class OllamaOptions
    {
        public string BaseAddress { get; set; } = "http://localhost:11434";
        public string Model { get; set; } = "tinyllama";
    }
}
