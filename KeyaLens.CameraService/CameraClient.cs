using Reactive.Bindings;
using Plugin.Media;
using System;

namespace KeyaLens.CameraService
{
    public class CameraClient : ICameraClient
    {
        public ReactiveProperty<string> ImageURL { get; set; } = new ReactiveProperty<string>("");

        public async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "KeyaLens",
                Name = $"{DateTime.Now.ToString()}.jpg"
            });

            if (file == null)
                return;

            ImageURL.Value = file.Path;
        }
    }
}
