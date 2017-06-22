using KeyaLens.DataModel;
using Reactive.Bindings;

namespace KeyaLens.UseCase
{
    public interface IKeyakiFaceAnalyzeUseCase
    {
        ReactiveProperty<bool> IsAnalyzing { get; set; }
        ReadOnlyReactiveCollection<PredictionResultModel> MemberDataList { get; set; }

        void FaceAnalyze();
    }
}