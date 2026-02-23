using HexMaster.FloodRush.App.Pages;

namespace HexMaster.FloodRush.App;

public partial class AppShell : Shell
{
	public AppShell(MainPage mainPage)
	{
		InitializeComponent();

		// Register routes for navigation
		Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));

		// Set the main page content
		Items.Clear();
		Items.Add(new ShellContent
		{
			Title = "Home",
			Route = "MainPage",
			Content = mainPage
		});
	}
}
