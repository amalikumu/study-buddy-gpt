using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace studyBuddy.Core.Services.VectorStorage
{
    public interface IVectorStorageService
    {        
        Task<bool> CreateCollectionAsync(int vectorSize);
        Task<bool> UpsertPointsAsync(List<VectorPoint> points);
    }
}
