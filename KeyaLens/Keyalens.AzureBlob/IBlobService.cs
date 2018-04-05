using System.IO;
using System.Threading.Tasks;

namespace Keyalens.AzureBlob
{
    public interface IBlobService
    {
        Task<string> UploadAsync(Stream imageStream);
    }
}