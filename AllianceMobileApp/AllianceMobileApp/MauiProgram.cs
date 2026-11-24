using AllianceMobileApp.ViewModel;
using Microsoft.Extensions.Logging;
using Refit;
using Microsoft.Extensions.Http;
using ZXing.Net.Maui.Controls;
using CommunityToolkit.Mvvm;
using AllianceMobileApp.Utility;
using CommunityToolkit.Maui;

namespace AllianceMobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            //builder.Services.AddRefitGeneratedClient<IInventoryService>()
            //    .ConfigureHttpClient(client =>
            //    {
            //        client.BaseAddress = new Uri("https://localhost:7170/");
            //        client.DefaultRequestHeaders.Add("Accept", "application/json");
            //    });

            //Leaving This here for Posterity, if we ever have issues with too many calls flooding the sockets implementing this *may(??)* Help
#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            return builder.Build();
        }
    }
}
