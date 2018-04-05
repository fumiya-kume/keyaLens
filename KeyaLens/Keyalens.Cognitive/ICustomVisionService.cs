using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Cognitive.CustomVision.Prediction.Models;

namespace Keyalens.Cognitive
{
    public interface ICustomVisionService
    {
        Task<IEnumerable<Prediction>> PreditionAsync(string imageUrl);
    }
}