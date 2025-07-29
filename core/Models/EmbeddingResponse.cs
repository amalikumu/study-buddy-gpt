using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace studyBuddy.Core.Models
{
    public class EmbeddingResponse
    {
        public List<List<float>> Embeddings { get; set; } = new();
    }
}
