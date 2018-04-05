using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
//using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Keyalens.Cognitive
{
    public class CustomVisionService : ICustomVisionService
    {
        public async Task<IEnumerable<Prediction>> PreditionAsync(string imageUrl)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { Url = imageUrl }), Encoding.UTF8, "application/json");
                const string predictionKey = "85d4fb9d76f74ddf9098ea6a5a15b6d0";
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

                const string RequestUri =
                    "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.1/Prediction/733f47d4-602b-4311-981e-af547501e759/url?iterationId=4212cbc5-b463-4826-9af9-dd18c173fd9c";
                var predictionResult = await client.PostAsync(RequestUri, content);
                var prefictionResultJson = await predictionResult.Content.ReadAsStringAsync();
                var customVisonPredictionResult = JsonConvert.DeserializeObject<CustomVisonPredictionResult>(prefictionResultJson);
                return customVisonPredictionResult.Predictions;
            }
        }
    }

    public class CustomVisonPredictionResult
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
}
