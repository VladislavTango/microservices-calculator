using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.Extensions.Logging;

namespace PraktikaMauiBlazor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Services.AddBlazoredSessionStorage();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
