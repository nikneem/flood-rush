using HexMaster.FloodRush.App.Models;
using HexMaster.FloodRush.App.Services;
using System.Text.RegularExpressions;

namespace HexMaster.FloodRush.App.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly ISettingsService _settingsService;
    private readonly GameSettings _settings;
    private readonly Regex _playerNameValidation = new Regex(@"^[a-zA-Z0-9\s_\-]*$", RegexOptions.Compiled);

    public SettingsPage(ISettingsService settingsService)
    {
        InitializeComponent();
        _settingsService = settingsService;
        _settings = _settingsService.LoadSettings();
        BindingContext = _settings;
    }

    private void OnPlayerNameChanged(object? sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            PlayerNameError.IsVisible = false;
            _settings.PlayerName = e.NewTextValue;
            _settingsService.SaveSettings(_settings);
            return;
        }

        if (_playerNameValidation.IsMatch(e.NewTextValue))
        {
            PlayerNameError.IsVisible = false;
            _settings.PlayerName = e.NewTextValue;
            _settingsService.SaveSettings(_settings);
        }
        else
        {
            PlayerNameError.IsVisible = true;
            // Revert to the old value
            PlayerNameEntry.Text = e.OldTextValue;
        }
    }

    private void OnMusicVolumeChanged(object? sender, ValueChangedEventArgs e)
    {
        var volume = (int)e.NewValue;
        _settings.MusicVolume = volume;
        MusicVolumeLabel.Text = volume.ToString();
        _settingsService.SaveSettings(_settings);
    }

    private void OnSoundVolumeChanged(object? sender, ValueChangedEventArgs e)
    {
        var volume = (int)e.NewValue;
        _settings.SoundVolume = volume;
        SoundVolumeLabel.Text = volume.ToString();
        _settingsService.SaveSettings(_settings);
    }

    private async void OnBackButtonClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        // Handle hardware back button
        Dispatcher.Dispatch(async () => await Navigation.PopModalAsync());
        return true;
    }
}
