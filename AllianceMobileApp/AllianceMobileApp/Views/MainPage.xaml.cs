using AllianceMobileApp.ViewModel;
using ZXing.Net.Maui;

namespace AllianceMobileApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;
        }
    }
}
