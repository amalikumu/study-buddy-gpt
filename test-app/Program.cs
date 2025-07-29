// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using studyBuddy.Core.Services;
using studyBuddy.DocumentChunker;

// Config setup
var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false);
var config = builder.Build();

// File path input
Console.WriteLine("Enter path to the document:");
var filePath = Console.ReadLine();

if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
{
    Console.WriteLine("Invalid file path.");
    return;
}

try
{
    var extractor = TextExtractorFactory.CreateExtractor(filePath);
    var text = extractor.ExtractText(filePath);
    var chunks = TextChunker.ChunkTextBySentences(text, maxCharsPerChunk: 1000);

    var embeddingService = new LocalEmbeddingService();

    int i = 1;
    foreach (var chunk in chunks)
    {
        Console.WriteLine($"🔄 Getting embedding for chunk {i}...");
        var embedding = await embeddingService.GetEmbeddingAsync(chunk);
        Console.WriteLine($"✅ Chunk {i} embedded (Vector Size: {embedding.Count})\n");
        // 👉 You can save these embeddings with metadata here
        i++;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
}