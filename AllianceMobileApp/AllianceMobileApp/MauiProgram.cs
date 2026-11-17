using AllianceMobileApp.ViewModel;
using Microsoft.Extensions.Logging;
using Refit;
using Microsoft.Extensions.Http;
using ZXing.Net.Maui.Controls;
using AllianceMobileApp.Utility;

namespace AllianceMobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddHttpClient("AllianceInventory", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7170/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddRefitGeneratedClient<IInventoryService>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri("https://localhost:7170/");
                });
#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            return builder.Build();
        }
    }
}
