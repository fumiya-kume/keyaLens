using System.Threading.Tasks;
using Reactive.Bindings;

namespace keyalens.Usecase
{
    public interface IFaceAnalyzeUsecase
    {
        ReactiveProperty<string> PredictName { get; set; }
        ReactiveProperty<string> TakeImageUrl { get; set; }
        ReactiveProperty<bool> IsLoading { get; set; }

        Task Analyze();
        void ResetAnalyzeResult();
    }
}