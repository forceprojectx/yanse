using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using yanse.ViewModels;

namespace yanse
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
                .UseMauiCommunityToolkitCamera()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            RegisterViewsAndViewModels(builder.Services);
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }


        static void RegisterViewsAndViewModels(in IServiceCollection services)
        {
            services.AddTransient<MainPage, MainPageViewModel>();
        }
    }
}
