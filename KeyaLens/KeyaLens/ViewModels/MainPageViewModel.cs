using KeyaLens.CameraService;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using Reactive.Bindings;
using System.Reactive.Linq;
using System.IO;
//using KeyaLens.CustomVisionService;
using System.Linq;

namespace KeyaLens.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private readonly ICameraClient cameraClient;
        //private readonly ICustomVisionClient customVisionClient;

        public ReactiveProperty<string> PhotoURL { get; set; } = new ReactiveProperty<string>();
        public ReactiveCollection<string> TagList { get; set; } = new ReactiveCollection<string>();

        public ReactiveCommand TakePhotoCommand { get; private set; } = new ReactiveCommand();
        

        public MainPageViewModel(ICameraClient cameraclient/*, ICustomVisionClient customvisionclient*/)
        {
            cameraClient = cameraclient;
            //customVisionClient = customvisionclient;

            PhotoURL = cameraClient.ImageURL;
            
            //TagList.AddRangeOnScheduler(customVisionClient.ImageTagList.Select(tag => tag.TagName));

            TakePhotoCommand
                .Subscribe(_ =>
                {
                    cameraClient.TakePhoto();
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
