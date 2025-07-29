
namespace studyBuddy.Core.Services
{
    public interface IEmbeddingService
    {
        Task<List<float>> GetEmbeddingAsync(string text);
    }
}