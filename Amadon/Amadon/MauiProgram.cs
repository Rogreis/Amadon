using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazorise.RichTextEdit;

using System.Text.Json;


/*
 * Execution order:
 * 
 * 
    Program.cs: 
        The application starts from the Main method in the Program.cs file. 
        This is where the app's host builder is configured, and the MauiApp is created.
        (there are one specific for MAc and IOS)

    Startup.cs (optional): 
        If a Startup class is used in the application, it will be called after the host builder is configured. 
        The Startup class typically contains app configuration, services registration, and middleware setup. 
        However, starting with .NET MAUI Preview 10, the Startup class is optional, and you can perform these configurations directly in the Program.cs file.

    MauiProgram.cs (for template-based projects): 
        For projects created using the .NET MAUI Blazor templates, the MauiProgram class is an intermediate class that contains 
        additional configuration options for Blazor WebView. 
        This class may not be present in projects that are not created from a template.

    App.xaml(.cs): The App.xaml and App.xaml.cs files define the shared resources and application-level logic for your app. This is where you can initialize your MainPage and set it as the MainPage of your app.

MainPage.xaml(.cs): The MainPage.xaml and its code-behind file, MainPage.xaml.cs, define the layout and behavior of the first window displayed by the application. In a .NET MAUI Blazor application, this page typically hosts the BlazorWebView control that renders the Blazor components.

_Imports.razor: This file contains common using statements and component imports that are shared across your Blazor components.

App.razor: This is the root component of your Blazor application. It typically contains the Router component, which manages the routing for your app.

Index.razor: The Index.razor file is the first Blazor component displayed by default, as specified in the App.razor file. It usually serves as the home page for your application.

Once the Index.razor component is rendered, the first window of the .NET MAUI Blazor application is displayed. Keep in mind that the specific details may vary depending on your project configuration and structure.




 * 
 * 
 * */


namespace Amadon
{
    // Initial config for Blazorise https://blazorise.com/docs/start/#4a-blazor-webassembly
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();

            //InitializationService.InitLogger(builder);


            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddBlazoredLocalStorage(config =>
            {
                config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                //config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                //config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                config.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services
                .AddBlazoriseRichTextEdit();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
		//builder.Logging.AddDebug();
#endif

            builder.Services
                .AddBlazorise(options =>
                {
                    options.Immediate = true;
                })
                .AddBootstrap5Providers()
                .AddFontAwesomeIcons();

            return builder.Build();
        }
    }
}