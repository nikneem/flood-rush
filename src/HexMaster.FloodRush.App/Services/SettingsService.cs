using HexMaster.FloodRush.App.Models;
using System.Text.Json;

namespace HexMaster.FloodRush.App.Services;

public interface ISettingsService
{
    GameSettings LoadSettings();
    void SaveSettings(GameSettings settings);
}

public class SettingsService : ISettingsService
{
    private const string SettingsKey = "game_settings";
    private GameSettings? _cachedSettings;

    public GameSettings LoadSettings()
    {
        if (_cachedSettings != null)
        {
            return _cachedSettings;
        }

        var settingsJson = Preferences.Get(SettingsKey, string.Empty);

        if (string.IsNullOrEmpty(settingsJson))
        {
            _cachedSettings = new GameSettings();
            SaveSettings(_cachedSettings);
            return _cachedSettings;
        }

        try
        {
            _cachedSettings = JsonSerializer.Deserialize<GameSettings>(settingsJson) ?? new GameSettings();
        }
        catch
        {
            _cachedSettings = new GameSettings();
        }

        return _cachedSettings;
    }

    public void SaveSettings(GameSettings settings)
    {
        _cachedSettings = settings;
        var settingsJson = JsonSerializer.Serialize(settings);
        Preferences.Set(SettingsKey, settingsJson);
    }
}
