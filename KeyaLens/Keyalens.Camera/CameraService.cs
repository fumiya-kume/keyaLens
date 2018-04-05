using System;
using System.Globalization;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace Keyalens.Camera
{
    public class CameraService : ICameraService
    {
        public async Task<string> TakePhotoAsync()
        {
            var initResult = await CrossMedia.Current.Initialize();
            if (!initResult)
            {
                return "";
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                throw new NotSupportedException("You should Set up camera");
            }

            // 撮影し、保存したファイルを取得
            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "keyalens",
                Name = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            });

            // 保存したファイルのパスを取得
            return photo.Path;
        }
    }
}
