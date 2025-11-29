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
            try
            {
                // Add unhandled exception handler
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
                TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in App constructor: {ex}");
                System.IO.File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "amadon_error.txt"), $"Error in App constructor: {ex}\r\n{ex.StackTrace}");
                throw;
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unhandled Exception: {ex}");
                System.IO.File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "amadon_error.txt"), $"Unhandled Exception: {ex}\r\n{ex.StackTrace}");
            }
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Unobserved Task Exception: {e.Exception}");
            System.IO.File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "amadon_error.txt"), $"Unobserved Task Exception: {e.Exception}\r\n{e.Exception.StackTrace}");
            e.SetObserved();
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