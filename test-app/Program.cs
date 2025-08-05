// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using studyBuddy.Core.Services;
using studyBuddy.Core.Services.Embedding;
using studyBuddy.Core.Services.LLM;
using studyBuddy.Core.Services.VectorStorage;
using studyBuddy.DocumentChunker;

// Config setup
var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false);
var config = builder.Build();

var embeddingService = new LocalEmbeddingService();
var qdrantStorage = new QdrantVectorStorage("http://localhost:6333", "Books");
var llm = new OllamaService();

Console.WriteLine ("Welcome to the Study Buddy GPT!");
Console.WriteLine("Do you want to 1. Upload Content or 2. Search!");

var choice = Console.ReadLine();

if (choice != "1" && choice != "2")
{
    Console.WriteLine("Invalid choice. Please enter 1 or 2.");
    return;
}
if (choice == "1")
{
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
        var file = new FileInfo(filePath);
        var extractor = TextExtractorFactory.CreateExtractor(filePath);
        var text = extractor.ExtractText(filePath);
        var chunks = TextChunker.ChunkTextBySentences(text, maxCharsPerChunk: 200);

        await qdrantStorage.CreateCollectionAsync(384); // Once only


        int i = 1;
        foreach (var chunk in chunks)
        {
            var embedding = await embeddingService.GetEmbeddingAsync(chunk);

            var point = new VectorPoint
            {
                Vector = embedding.ToArray(),
                Payload = new Dictionary<string, object>
            {
                { "text", chunk },
                { "source", file.Name },
                { "chunk_index", i }
            }
            };

            await qdrantStorage.UpsertPointsAsync(new List<VectorPoint> { point });
            i++;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error: {ex.Message}");
    }
}
else if (choice == "2")
{
    while(true)
    {   // Search input
        Console.WriteLine("Enter your search query:");
        var query = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(query))
        {
            Console.WriteLine("Invalid search query.");
            break;
        }
        try
        {
            var queryEmbedding = await embeddingService.GetEmbeddingAsync(query);
            var results = await qdrantStorage.SearchAsync(queryEmbedding.ToArray(), topK: 3);
            string context = string.Join("\n\n", results); // combine top n chunks
            string answer = await llm.GetAnswerAsync(context, query);
            Console.WriteLine("AI Answer:");
            Console.WriteLine(answer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }
    Console.WriteLine("Thank You");
}