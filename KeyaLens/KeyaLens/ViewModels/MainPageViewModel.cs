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

namespace KeyaLens.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private readonly ICameraClient cameraClient;
        private readonly ICustomVisionClient customVisionClient;
        private readonly IKeyakiMemberClient keyakiMembeClient;

        public ReactiveProperty<string> PhotoURL { get; set; } = new ReactiveProperty<string>();
        public ReadOnlyReactiveCollection<string> TagList { get; set; }
        public ReactiveProperty<string> TappedMember { get; set; } = new ReactiveProperty<string>();


        public ReactiveCommand TakePhotoCommand { get; private set; } = new ReactiveCommand();


        public MainPageViewModel(ICameraClient cameraclient, ICustomVisionClient customvisionclient, IKeyakiMemberClient keyakimemberclient)
        {
            cameraClient = cameraclient;
            customVisionClient = customvisionclient;
            keyakiMembeClient = keyakimemberclient;

            PhotoURL = cameraClient.ImageURL;

            PhotoURL.Subscribe(URL =>
            {
                customVisionClient.PredicateImageFromMemoryStream(URL);
            });

            TagList = customVisionClient.ImageTagList.ToReadOnlyReactiveCollection(tag => tag.TagName);

            TakePhotoCommand
                .Subscribe(_ =>
                {
                    cameraClient.TakePhoto();
                });

            TappedMember
                .Where(memberName => !string.IsNullOrWhiteSpace(memberName))
                .Subscribe(memberName =>
                {
                    keyakiMembeClient.GetMemberNameAsync();

                    var URL = keyakiMembeClient.MemberCollection
                    .First(memberInfo => memberInfo.Name == memberName).memberPageURL;
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
