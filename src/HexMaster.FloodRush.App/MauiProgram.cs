using Microsoft.Extensions.Logging;
using HexMaster.FloodRush.App.Services;
using HexMaster.FloodRush.App.Pages;
using HexMaster.FloodRush.App.ViewModels;

namespace HexMaster.FloodRush.App;

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
				fonts.AddFont("Ranchers-Regular.ttf", "Ranchers");
			});

		// Register services
		builder.Services.AddSingleton<ISettingsService, SettingsService>();

		// Register ViewModels
		builder.Services.AddTransient<GamePageViewModel>();

		// Register pages
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddTransient<GamePage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
