using Reactive.Bindings;

namespace KeyaLens.CameraService
{
    public interface ICameraClient
    {
        ReactiveProperty<string> ImageURL { get; set; }

        void TakePhoto();
    }
}