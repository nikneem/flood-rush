using System.ComponentModel;
using System.Runtime.CompilerServices;
using HexMaster.FloodRush.App.Services;
using HexMaster.FloodRush.App.Models;
using HexMaster.FloodRush.Game.DomainModels;

namespace HexMaster.FloodRush.App.ViewModels;

public class GamePageViewModel : INotifyPropertyChanged
{
    private readonly ISettingsService _settingsService;

    private int _currentLevel = 1;
    private TimeSpan _playTime = TimeSpan.Zero;
    private int _topScore;
    private int _currentScore;
    private string _playerName = string.Empty;
    private Level? _level;

    public GamePageViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        InitializeGame();
    }

    public int CurrentLevel
    {
        get => _currentLevel;
        set
        {
            if (_currentLevel != value)
            {
                _currentLevel = value;
                OnPropertyChanged();
            }
        }
    }

    public string PlayTime
    {
        get => _playTime.ToString(@"mm\:ss");
    }

    public int TopScore
    {
        get => _topScore;
        set
        {
            if (_topScore != value)
            {
                _topScore = value;
                OnPropertyChanged();
            }
        }
    }

    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            if (_currentScore != value)
            {
                _currentScore = value;
                OnPropertyChanged();
            }
        }
    }

    public string PlayerName
    {
        get => _playerName;
        set
        {
            if (_playerName != value)
            {
                _playerName = value;
                OnPropertyChanged();
            }
        }
    }

    public Level? Level
    {
        get => _level;
        set
        {
            if (_level != value)
            {
                _level = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void InitializeGame()
    {
        var settings = _settingsService.LoadSettings();

        // Generate unique player name if not set or default
        if (string.IsNullOrWhiteSpace(settings.PlayerName) || settings.PlayerName == "Player")
        {
            settings.PlayerName = GeneratePlayerName();
            _settingsService.SaveSettings(settings);
        }

        PlayerName = settings.PlayerName;

        // Initialize game state
        CurrentLevel = settings.CurrentLevel;
        _playTime = TimeSpan.Zero;
        TopScore = 0; // TODO: Load from high score service
        CurrentScore = 0;

        // Load the level
        Level = Game.DomainModels.Level.Load(settings.CurrentLevel);
    }

    private string GeneratePlayerName()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var uniqueId = new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());

        return $"Player_{uniqueId}";
    }

    public void UpdatePlayTime(TimeSpan time)
    {
        _playTime = time;
        OnPropertyChanged(nameof(PlayTime));
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
