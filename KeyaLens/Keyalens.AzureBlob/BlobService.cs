using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;

namespace Keyalens.AzureBlob
{
    public class BlobService : IBlobService
    {
        public async Task<string> UploadAsync(Stream imageStream)
        {
            const string StorageConectionString =
                "DefaultEndpointsProtocol=https;AccountName=keyalens;AccountKey=/B1vCIyrNAfJPxEFHZp5kRJbUk96eLGrPjpmUlXBVhVCJp614UzcfmGpVuDuvw4ssRu8YVry7A1YVI8o9gAduw==;EndpointSuffix=core.windows.net";
            var cloudAccount = CloudStorageAccount.Parse(StorageConectionString);
            var cloudBlobClient = cloudAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("customvisionimage");
            await cloudBlobContainer.CreateIfNotExistsAsync();
            var blockBlobReference = cloudBlobContainer.GetBlockBlobReference(Guid.NewGuid().ToString() + DateTime.Now.Ticks + ".jpg");
            await blockBlobReference.UploadFromStreamAsync(imageStream);
            return blockBlobReference.Uri.ToString();
        }
    }
}
