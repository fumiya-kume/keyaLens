using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using keyalens.Views;
using Reactive.Bindings;

namespace keyalens.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ReactiveCommand NavigateCameraPage { get; set; } = new ReactiveCommand();

        public MainPageViewModel(INavigationService navigationService) 
            : base (navigationService)
        {
            _navigationService = navigationService;
            Title = "Main Page";

            NavigateCameraPage
                .Subscribe(() => _navigationService.NavigateAsync(nameof(CameraPage)));
        }
    }
}
