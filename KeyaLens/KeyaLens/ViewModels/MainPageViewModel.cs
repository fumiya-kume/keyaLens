using KeyaLens.CameraService;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using Reactive.Bindings;
using System.Reactive.Linq;
using System.IO;
using KeyaLens.CustomVisionService;
using System.Linq;
using System.Collections.Generic;
using KeyaLens.KeyakiMemberService;
using Xamarin.Forms;
using KeyaLens.DataModel;

namespace KeyaLens.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private readonly ICameraClient cameraClient;
        private readonly ICustomVisionClient customVisionClient;
        private readonly IKeyakiMemberClient keyakiMembeClient;

        public ReactiveProperty<string> PhotoURL { get; set; } = new ReactiveProperty<string>();
        public ReadOnlyReactiveCollection<string> TagList { get; set; }
        public ReadOnlyReactiveCollection<PredictionResultModel> MemberInfoList { get; set; }
        public ReactiveProperty<PredictionResultModel> TappedMember { get; set; } = new ReactiveProperty<PredictionResultModel>();

        public ReactiveCommand TakePhotoCommand { get; private set; } = new ReactiveCommand();


        public MainPageViewModel(ICameraClient cameraclient, ICustomVisionClient customvisionclient, IKeyakiMemberClient keyakimemberclient)
        {
            cameraClient = cameraclient;
            customVisionClient = customvisionclient;
            keyakiMembeClient = keyakimemberclient;

            PhotoURL = cameraClient.ImageURL;

            PhotoURL.Subscribe(URL => { customVisionClient.PredicateImageFromMemoryStream(URL); });

            TagList = customVisionClient.ImageTagList.ToReadOnlyReactiveCollection(tag => tag.TagName);

            MemberInfoList = TagList.ToReadOnlyReactiveCollection(MemberName =>
            {
                var MemberInfo = keyakiMembeClient.MemberCollection.First(member => member.Name == MemberName);
                var probablyRank = customVisionClient.ImageTagList.First(tag => tag.TagName == MemberName).Probably;
                return new PredictionResultModel() { Name = MemberName, ProfileImageURL = MemberInfo.ProfileImageURL, ProfileLinkImage = MemberInfo.memberPageURL, ProbablyRank = probablyRank };
            });

            TakePhotoCommand.Subscribe(_ => { cameraClient.TakePhoto(); });

            TappedMember
                .Where(memberName => memberName != null && !string.IsNullOrWhiteSpace(memberName.Name))
                .Subscribe(memberName =>
                {
                    var URL = keyakiMembeClient.MemberCollection.First(memberInfo => memberInfo.Name == memberName.Name).memberPageURL;
                    Device.OpenUri(new Uri(URL));
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
