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
            if (string.IsNullOrWhiteSpace(ImageURL)) return;

            var Image = await FileSystem.Current.GetFileFromPathAsync(ImageURL.Replace("\\\\", "\\"));

            var ImageStream = await Image.OpenAsync(FileAccess.ReadAndWrite);

            var result = GetCustomVisionEndPoint().PredictImage(GetProjectGUID(), ImageStream);
            ImageTagList.Clear();
            var TagInfoList = result.Predictions
                .Select(prefiction => new CustomVisionTagInfo() { TagName = prefiction.Tag, Probably = prefiction.Probability })
                .ToList();

            foreach (var item in TagInfoList)
            {
                ImageTagList.Add(item);
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
