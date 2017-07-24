using Microsoft.Cognitive.CustomVision;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Cognitive.CustomVision.Models;
using PCLStorage;
using Reactive.Bindings;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace KeyaLens.CustomVisionService
{
    public class CustomVisionClient : ICustomVisionClient
    {
        public ReactiveCollection<CustomVisionTagInfo> ImageTagList { get; set; } = new ReactiveCollection<CustomVisionTagInfo>();

        /// <summary>
        /// 判定する
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public async void PredicateImageFromMemoryStream(string ImageURL)
        {
            if (string.IsNullOrWhiteSpace(ImageURL)) return;


            var result = ParsePredictJson(await MakePredictionRequest(ImageURL));

            ImageTagList.Clear();
            var TagInfoList = result.Predictions
                .Select(prefiction => new CustomVisionTagInfo() { TagName = prefiction.Tag, Probably = prefiction.Probability })
                .Where(prediction => prediction.TagName != "欅坂46")
                .ToList();
            
            foreach (var item in TagInfoList)
            {
                ImageTagList.Add(item);
            }
        }

        private predictResult ParsePredictJson(string json) => JsonConvert.DeserializeObject<predictResult>(json);

        public class predictResult
        {
            public string Id { get; set; }
            public string Project { get; set; }
            public string Iteration { get; set; }
            public DateTime Created { get; set; }
            public Prediction[] Predictions { get; set; }
        }

        public class Prediction
        {
            public string TagId { get; set; }
            public string Tag { get; set; }
            public float Probability { get; set; }
        }


        static async Task<string> MakePredictionRequest(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid subscription key.
            client.DefaultRequestHeaders.Add("Prediction-Key", "85d4fb9d76f74ddf9098ea6a5a15b6d0");

            // Prediction URL - replace this example URL with your valid prediction URL.
            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/233dc05e-f8e2-4f1c-a02d-b69fd8efd53c/image?iterationId=3c5b944d-a3e3-4ef9-8036-0f33d6e224f9";

            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored image.
            byte[] byteData = await ReadPhotFromLocalStorageAsync(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        private static async Task<byte[]> ReadPhotFromLocalStorageAsync(string LocalPhotoPath)
        {
            var Image = await FileSystem.Current.GetFileFromPathAsync(LocalPhotoPath.Replace("\\\\", "\\"));
            var ImageStream = await Image.OpenAsync(FileAccess.ReadAndWrite);
            BinaryReader binaryReader = new BinaryReader(ImageStream);
            return binaryReader.ReadBytes((int)ImageStream.Length);
        }

        /// <summary>
        ///     Custom Vision Service の エンドポイント
        /// </summary>
        /// <returns></returns>
        static PredictionEndpoint GetCustomVisionEndPoint()
        {
            var PrimaryKey = GetTrainingApi().GetAccountInfo().Keys.PredictionKeys.PrimaryKey;
            var PredictionEndPointCredetials = new PredictionEndpointCredentials(PrimaryKey);
            var Endpoint = new PredictionEndpoint(PredictionEndPointCredetials);
            return Endpoint;
        }

        /// <summary>
        /// プロジェクトの GUID を取得する
        /// </summary>
        /// <returns>プロジェクトのGUID</returns>
        static Guid GetProjectGUID()
        {
            var projects = GetTrainingApi().GetProjects();
            return projects.Count(project => project.Name == "KEYAKI") == 0 ?
                GetTrainingApi().CreateProject("KEYAKI").Id :
                projects.First(project => project.Name == "KEYAKI").Id;
        }

        /// <summary>
        /// TrainingAPI のインスタンスを取得する
        /// </summary>
        /// <returns>Training API のインスタンス</returns>
        static TrainingApi GetTrainingApi()
        {
            var trainingKey = "d2914ae93a514efaada50aed9b0f9173";
            TrainingApiCredentials trainingCredentials = new TrainingApiCredentials(trainingKey);
            return new TrainingApi(trainingCredentials);
        }
    }
}
