using Prism.Unity;
using KeyaLens.Views;
using Xamarin.Forms;
using KeyaLens.CameraService;
using Microsoft.Practices.Unity;
//using KeyaLens.CustomVisionService;

namespace KeyaLens
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();

            Container.RegisterType<ICameraClient,CameraClient>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<ICustomVisionClient, CustomVisionClient>(new ContainerControlledLifetimeManager());
        }
    }
}
