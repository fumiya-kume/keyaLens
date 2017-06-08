using System.Linq;
using KeyaLens.DataModel;
using KeyaLens.CustomVisionService;
using KeyaLens.KeyakiMemberService;

namespace KeyaLens.Translator
{
    public class PredictionResultTranslator : IPredictionResultTranslator
    {
        private readonly ICustomVisionClient customVisionClient;
        private readonly IKeyakiMemberClient keyakiMembeClient;

        public PredictionResultTranslator(ICustomVisionClient customvisionclient, IKeyakiMemberClient keyakimemberclient)
        {
            customVisionClient = customvisionclient;
            keyakiMembeClient = keyakimemberclient;
        }

        public PredictionResultModel Translate(string Name)
        {
            var MemberInfo = keyakiMembeClient.MemberCollection.First(member => member.Name == Name);
            var PredictionResult = customVisionClient.ImageTagList.First(tag => tag.TagName == Name);

            PredictionResult.Probably = PredictionResult.Probably * 100;

            return new PredictionResultModel() { Name = Name, ProfileImageURL = MemberInfo.ProfileImageURL, ProfileLinkImage = MemberInfo.memberPageURL, ProbablyRank = PredictionResult.Probably };
        }

        static PredictionResultModel ProbablyToPercent(PredictionResultModel model)
        {
            model.ProbablyRank = model.ProbablyRank * 100;
            return model;
        }
    }
}
