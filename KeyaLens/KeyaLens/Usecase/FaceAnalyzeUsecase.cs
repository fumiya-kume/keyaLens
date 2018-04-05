using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Keyalens.AzureBlob;
using Keyalens.Camera;
using Keyalens.Cognitive;
using Reactive.Bindings;

namespace keyalens.Usecase
{
    public class FaceAnalyzeUsecase : IFaceAnalyzeUsecase
    {
        private readonly ICameraService _cameraService;
        private readonly IBlobService _blobService;
        private readonly ICustomVisionService _customVisionService;

        public ReactiveProperty<string> TakeImageUrl { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> PredictName { get; set; } = new ReactiveProperty<string>();

        public FaceAnalyzeUsecase(ICameraService cameraService, IBlobService blobService, ICustomVisionService customVisionService)
        {
            _cameraService = cameraService;
            _blobService = blobService;
            _customVisionService = customVisionService;
        }

        public async Task Analyze()
        {
            var cameraUrl = await _cameraService.TakePhotoAsync();

            TakeImageUrl.Value = cameraUrl;

            var fileByte = File.ReadAllBytes(cameraUrl);
            using (var imageStream = new MemoryStream(fileByte))
            {
                var blobUrl = await _blobService.UploadAsync(imageStream);

                var predictResult = await _customVisionService.PreditionAsync(blobUrl);
                PredictName.Value = predictResult.OrderBy(prediction => prediction.Probability).First().Tag;
            }
        }
    }
}
