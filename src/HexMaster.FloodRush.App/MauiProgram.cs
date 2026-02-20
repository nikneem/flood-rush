using Microsoft.Extensions.Logging;
using HexMaster.FloodRush.App.Services;

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

		// Register pages
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<AppShell>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
