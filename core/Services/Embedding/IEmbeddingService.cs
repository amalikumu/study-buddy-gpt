
namespace studyBuddy.Core.Services.Embedding
{
    public interface IEmbeddingService
    {
        Task<List<float>> GetEmbeddingAsync(string text);
    }
}