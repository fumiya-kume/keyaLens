using System.Collections.Generic;
using System.IO;
using Microsoft.Cognitive.CustomVision.Models;
using Reactive.Bindings;

namespace KeyaLens.CustomVisionService
{
    public interface ICustomVisionClient
    {
        ReactiveCollection<CustomVisionTagInfo> ImageTagList { get; set; }
        void PredicateImageFromMemoryStream(string ImageURL);
    }
}