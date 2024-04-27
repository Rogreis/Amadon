using Amadon.Services;

namespace Amadon
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            TrackService.Dummy();
        }
    }
}