using HexMaster.FloodRush.App.Pages;
using HexMaster.FloodRush.App.Services;

namespace HexMaster.FloodRush.App;

public partial class MainPage : ContentPage
{
	private readonly ISettingsService _settingsService;

	public MainPage(ISettingsService settingsService)
	{
		InitializeComponent();
		_settingsService = settingsService;
	}

	private async void OnPlayNowClicked(object? sender, EventArgs e)
	{
		// TODO: Navigate to game page
		await DisplayAlert("Play Now", "Game will start here!", "OK");
	}

	private async void OnSettingsClicked(object? sender, EventArgs e)
	{
		var settingsPage = new SettingsPage(_settingsService);
		await Navigation.PushModalAsync(settingsPage);
	}
}
