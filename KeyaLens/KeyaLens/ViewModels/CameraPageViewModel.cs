using System;
using System.Linq;
using System.Reactive.Linq;
using keyalens.Usecase;
using Prism.Mvvm;
using Reactive.Bindings;
using Xamarin.Forms;

namespace keyalens.ViewModels
{
    public class CameraPageViewModel : BindableBase
    {
        private readonly IFaceAnalyzeUsecase _faceAnalyzeUsecase;

        public ReactiveProperty<string> TakePhotoUrl { get; set; }
        public ReadOnlyReactivePropertySlim<string> Predictname { get; set; }
        public ReadOnlyReactivePropertySlim<bool> HasPredictionResult { get; set; }
        public ReadOnlyReactivePropertySlim<string> CameraButtonImageUrl { get; set; }

        public ReactiveCommand TakePhotoCommand { get; set; } = new ReactiveCommand();
        public CameraPageViewModel(IFaceAnalyzeUsecase faceAnalyzeUsecase)
        {
            _faceAnalyzeUsecase = faceAnalyzeUsecase;

            TakePhotoUrl = _faceAnalyzeUsecase.TakeImageUrl;

            Predictname = _faceAnalyzeUsecase.PredictName.ToReadOnlyReactivePropertySlim();

            HasPredictionResult = Predictname.Select(s => !string.IsNullOrEmpty(s)).ToReadOnlyReactivePropertySlim();

            CameraButtonImageUrl = HasPredictionResult
                .Merge(_faceAnalyzeUsecase.IsLoading)
                .Select(b =>
                        _faceAnalyzeUsecase.IsLoading.Value
                            ? Application.Current.Resources.First(pair => pair.Key == "LoadingButtonImageUrl").Value.ToString()
                            : (HasPredictionResult.Value
                                ? Application.Current.Resources.First(pair => pair.Key == "resetButtonImageUrl").Value.ToString()
                                : Application.Current.Resources.First(pair => pair.Key == "startButtonImageUrl").Value.ToString()
                                ))
                        .ToReadOnlyReactivePropertySlim();

            TakePhotoCommand
                .Where(o => HasPredictionResult.Value && !string.IsNullOrEmpty(CameraButtonImageUrl.Value))
                .Subscribe(_ => _faceAnalyzeUsecase.ResetAnalyzeResult());

            TakePhotoCommand
                .Where(o => string.IsNullOrEmpty(Predictname.Value))
                .Subscribe(_ => _faceAnalyzeUsecase.Analyze());

        }
    }
}
