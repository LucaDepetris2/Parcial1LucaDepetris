using Microsoft.Extensions.Logging;
using SQLite;
using Parcial1LucaDepetris.Services;
using Parcial1LucaDepetris.ViewModels;
using Parcial1LucaDepetris.Views;

namespace Parcial1LucaDepetris
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // HttpClient y Servicio REST
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<IPostService, PostService>();

            // Repositorio SQLite local
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "favorites.db3");
            builder.Services.AddSingleton(_ => new SQLiteAsyncConnection(dbPath));
            builder.Services.AddSingleton<IFavoriteRepository, FavoriteRepository>();

            // ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<DetailViewModel>();
            builder.Services.AddTransient<FavoritesViewModel>();

            // Views
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<DetailPage>();
            builder.Services.AddTransient<FavoritesPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
