using System.Threading.Tasks;

namespace Keyalens.Camera
{
    public interface ICameraService
    {
        Task<string> TakePhotoAsync();
    }
}