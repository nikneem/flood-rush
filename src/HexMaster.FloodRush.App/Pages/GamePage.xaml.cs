using HexMaster.FloodRush.App.ViewModels;

namespace HexMaster.FloodRush.App.Pages;

public partial class GamePage : ContentPage
{
    private readonly GamePageViewModel _viewModel;
    private System.Timers.Timer? _gameTimer;
    private TimeSpan _elapsedTime;

    public GamePage(GamePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Initialize the playfield with the loaded level
        if (_viewModel.Level != null)
        {
            PlayField.InitializeLevel(_viewModel.Level);
        }

        // Load tiles progressively into the queue
        await NextTilesQueue.LoadTilesProgressivelyAsync();

        StartGameTimer();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private void StartGameTimer()
    {
        _elapsedTime = TimeSpan.Zero;
        _gameTimer = new System.Timers.Timer(1000); // Update every second
        _gameTimer.Elapsed += OnGameTimerElapsed;
        _gameTimer.Start();
    }

    private void StopGameTimer()
    {
        if (_gameTimer != null)
        {
            _gameTimer.Stop();
            _gameTimer.Elapsed -= OnGameTimerElapsed;
            _gameTimer.Dispose();
            _gameTimer = null;
        }
    }

    private void OnGameTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));

        // Update the UI on the main thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _viewModel.UpdatePlayTime(_elapsedTime);
        });
    }

    private async void OnExitButtonClicked(object? sender, EventArgs e)
    {
        // Confirm exit
        bool confirm = await DisplayAlertAsync(
            "Exit Game",
            "Are you sure you want to exit the current game? Your progress will be lost.",
            "Yes",
            "No");

        if (confirm)
        {
            StopGameTimer();
            await Shell.Current.GoToAsync("..");
        }
    }
}
