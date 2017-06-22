using KeyaLens.CameraService;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using Reactive.Bindings;
using System.Reactive.Linq;
using KeyaLens.CustomVisionService;
using KeyaLens.Translator;
using System.Linq;
using Xamarin.Forms;
using KeyaLens.DataModel;
using KeyaLens.UseCase;

namespace KeyaLens.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {

        private readonly IPredictionResultTranslator _PrefictionResutTranslator;
        private readonly IKeyakiFaceAnalyzeUseCase _keyakiFaceAnalyzeUseCase;

        public ReactiveProperty<bool> IsBusy { get; set; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<string> PhotoURL { get; set; } = new ReactiveProperty<string>();
        public ReadOnlyReactiveCollection<PredictionResultModel> MemberInfoList { get; set; }
        public ReactiveProperty<PredictionResultModel> TappedMember { get; set; } = new ReactiveProperty<PredictionResultModel>();

        public ReactiveCommand TakePhotoCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand NavigateLicensePage { get; set; } = new ReactiveCommand();

        public MainPageViewModel(IKeyakiFaceAnalyzeUseCase keyakiFaceAnalyzeUseCase,IPredictionResultTranslator prefictionresulttranslator)
        {
            _PrefictionResutTranslator = prefictionresulttranslator;
            _keyakiFaceAnalyzeUseCase = keyakiFaceAnalyzeUseCase;

            MemberInfoList = _keyakiFaceAnalyzeUseCase.MemberDataList.ToReadOnlyReactiveCollection();

            IsBusy = _keyakiFaceAnalyzeUseCase.IsAnalyzing;

            TakePhotoCommand.Subscribe(_ =>
            {
                _keyakiFaceAnalyzeUseCase.FaceAnalyze();
            });

            TappedMember
                .Where(memberName => memberName != null && !string.IsNullOrWhiteSpace(memberName.Name))
                .Subscribe(memberName =>
                {
                    var URL = _PrefictionResutTranslator.Translate(memberName.Name).ProfileLinkImage;
                    Device.OpenUri(new Uri(URL));
                });

            NavigateLicensePage.Subscribe(_ =>
            {
                Device.OpenUri(new Uri(@"https://fumiya-kume.github.io/InfomationOfCopyright/KeyaLens/UWP.V1.html"));
            });
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }
    }
}
