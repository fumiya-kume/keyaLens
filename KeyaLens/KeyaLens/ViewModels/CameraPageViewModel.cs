﻿using Prism.Mvvm;
using keyalens.Usecase;
using Reactive.Bindings;

namespace keyalens.ViewModels
{
    public class CameraPageViewModel : BindableBase
    {
        private readonly IFaceAnalyzeUsecase _faceAnalyzeUsecase;

        public ReactiveProperty<string> TakePhotoUrl { get; set; }
        public ReadOnlyReactivePropertySlim<string> Predictname { get; set; }

        public ReactiveCommand TakePhotoCommand { get; set; } = new ReactiveCommand();
        public CameraPageViewModel(IFaceAnalyzeUsecase faceAnalyzeUsecase)
        {
            _faceAnalyzeUsecase = faceAnalyzeUsecase;

            TakePhotoUrl = _faceAnalyzeUsecase.TakeImageUrl;

            Predictname = _faceAnalyzeUsecase.PredictName.ToReadOnlyReactivePropertySlim();

            TakePhotoCommand.Subscribe(() => { _faceAnalyzeUsecase.Analyze(); });
        }
    }
}