using Microsoft.Cognitive.CustomVision;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Cognitive.CustomVision.Models;
using PCLStorage;
using Reactive.Bindings;

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
            var Image = await FileSystem.Current.LocalStorage.GetFileAsync(ImageURL);

            byte[] buffer = new byte[100];
            using (System.IO.Stream stream = await Image.OpenAsync(FileAccess.ReadAndWrite))
            {
                stream.Write(buffer, 0, 100);
                var result = GetCustomVisionEndPoint().PredictImage(GetProjectGUID(), stream);
                ImageTagList.Clear();
                result.Predictions
                    .Select(prefiction => new CustomVisionTagInfo() { TagName = prefiction.Tag, Probably = prefiction.Probability })
                    .ToList()
                    .Select(info =>
                    {
                        ImageTagList.Add(info);
                        return 0;
                    });
            }
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
