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

namespace KeyaLens.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private readonly ICameraClient cameraClient;
        private readonly ICustomVisionClient customVisionClient;
        private readonly IPredictionResultTranslator PrefictionResutTranslator;

        public ReactiveProperty<string> PhotoURL { get; set; } = new ReactiveProperty<string>();
        public ReadOnlyReactiveCollection<PredictionResultModel> MemberInfoList { get; set; }
        public ReactiveProperty<PredictionResultModel> TappedMember { get; set; } = new ReactiveProperty<PredictionResultModel>();

        public ReactiveCommand TakePhotoCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand NavigateLicensePage { get; set; } = new ReactiveCommand();

        public MainPageViewModel(ICameraClient cameraclient, ICustomVisionClient customvisionclient, IPredictionResultTranslator prefictionresulttranslator)
        {
            cameraClient = cameraclient;
            customVisionClient = customvisionclient;
            PrefictionResutTranslator = prefictionresulttranslator;

            PhotoURL = cameraClient.ImageURL;

            PhotoURL.Subscribe(URL => { customVisionClient.PredicateImageFromMemoryStream(URL); });

            MemberInfoList = customVisionClient.ImageTagList.ToReadOnlyReactiveCollection(tag => PrefictionResutTranslator.Translate(tag.TagName));

            TakePhotoCommand.Subscribe(_ => { cameraClient.TakePhoto(); });

            TappedMember
                .Where(memberName => memberName != null && !string.IsNullOrWhiteSpace(memberName.Name))
                .Subscribe(memberName =>
                {
                    var URL = PrefictionResutTranslator.Translate(memberName.Name).ProfileLinkImage;
                    Device.OpenUri(new Uri(URL));
                });

            NavigateLicensePage.Subscribe(_ => {
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
