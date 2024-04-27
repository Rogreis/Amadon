using Microsoft.Maui.Controls;
using System;

namespace Amadon.Classes
{
    internal class WebViewPage : ContentPage
    {
        public WebViewPage(string url)
        {
            WebView webView = new()
            {
                Source = new UrlWebViewSource { Url = url },
            };

            // Create and set up the Grid
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            // Add the WebView to the Grid
            grid.Children.Add(webView);

            // Set the Grid as the content of the page
            Content = grid;
        }
    }
}
