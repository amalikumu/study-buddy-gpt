namespace studyBuddy.Core.Services.VectorStorage
{
    public class VectorPoint
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public float[] Vector { get; set; }
        public Dictionary<string, object> Payload { get; set; } = new();
    }
}