namespace HexMaster.FloodRush.App;

public partial class AppShell : Shell
{
	public AppShell(MainPage mainPage)
	{
		InitializeComponent();

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
