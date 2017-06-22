using KeyaLens.CameraService;
using KeyaLens.CustomVisionService;
using KeyaLens.DataModel;
using KeyaLens.Translator;
using Reactive.Bindings;
using System;

namespace KeyaLens.UseCase
{
    public class KeyakiFaceAnalyzeUseCase : IKeyakiFaceAnalyzeUseCase
    {
        private readonly ICameraClient cameraClient;
        private readonly ICustomVisionClient customVisionClient;
        private readonly IPredictionResultTranslator PrefictionResutTranslator;

        public ReactiveProperty<bool> IsAnalyzing { get; set; } = new ReactiveProperty<bool>(false);
        public ReadOnlyReactiveCollection<PredictionResultModel> MemberDataList { get; set; }

        public KeyakiFaceAnalyzeUseCase(ICameraClient cameraclient, ICustomVisionClient customvisionclient, IPredictionResultTranslator prefictionresulttranslator)
        {
            cameraClient = cameraclient;
            customVisionClient = customvisionclient;
            PrefictionResutTranslator = prefictionresulttranslator;

            cameraClient.ImageURL.Subscribe(URL => customVisionClient.PredicateImageFromMemoryStream(URL));

            MemberDataList = customVisionClient.ImageTagList.ToReadOnlyReactiveCollection(tag =>
            {
                IsAnalyzing.Value = false;
                return PrefictionResutTranslator.Translate(tag.TagName);
            });
        }

        public void FaceAnalyze()
        {
            IsAnalyzing.Value = true;
            cameraClient.TakePhoto();
        }
    }
}
