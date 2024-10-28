using Amadon.Services;


namespace Amadon
{
    /// <summary>
    /// Main app 
    /// also implements the lificycle events to store/read local configuration
    /// <see href="https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle"/>
    /// </summary>
    public partial class App : Application
    {

        private Window window;

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            if (MainPage != null)
            {
                MainPage.Title = "O Livro de Urântia"; 
                MainPage.Disappearing += MainPage_Disappearing;
            }
            AmadonEvents.OnSetWindowTitle += AmadonEvents_OnSetWindowTitle;

            // https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle

            //this.PageDisappearing += App_PageDisappearing;
        }

        private void AmadonEvents_OnSetWindowTitle(string text)
        {
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                window.Title = text;
            }
        }

        private void MainPage_Disappearing(object sender, EventArgs e)
        {
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            window = base.CreateWindow(activationState);

            window.Created += (s, e) =>
            {
                //SettingsService.Get();
            };

            window.Destroying += (s, e) => 
            {
                SettingsService.Store();
            };

            return window;
        }

        //private void App_PageDisappearing(object sender, Page e)
        //{
        //}

        protected override void OnSleep()
        {
            //SettingsService.Store();
            base.OnSleep();
        }

    }
}