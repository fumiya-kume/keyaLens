using keyalens.Usecase;
using Prism;
using Prism.Ioc;
using keyalens.ViewModels;
using keyalens.Views;
using Keyalens.AzureBlob;
using Keyalens.Camera;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using Keyalens.Cognitive;
using Prism.Navigation;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace keyalens
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync($"NavigationPage/{nameof(MainPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICameraService, CameraService>();
            containerRegistry.Register<ICustomVisionService, CustomVisionService>();
            containerRegistry.Register<IBlobService, BlobService>();
            containerRegistry.Register<IFaceAnalyzeUsecase, FaceAnalyzeUsecase>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<CameraPage>();
            containerRegistry.RegisterForNavigation<TutorialPage>();
        }
    }
}
