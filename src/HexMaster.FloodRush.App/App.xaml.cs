using Microsoft.Extensions.DependencyInjection;

namespace HexMaster.FloodRush.App;

public partial class App : Application
{
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		MainPage = serviceProvider.GetRequiredService<AppShell>();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = new Window(MainPage!)
		{
			Title = "Flood Rush"
		};

		return window;
	}
}