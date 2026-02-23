using HexMaster.FloodRush.Game.DomainModels;

namespace HexMaster.FloodRush.App.Pages;

public partial class LevelStartPage : ContentPage
{
    private TaskCompletionSource<bool>? _taskCompletionSource;

    public LevelStartPage()
    {
        InitializeComponent();
        _taskCompletionSource = new TaskCompletionSource<bool>();
    }

    public void SetLevelInfo(Level level)
    {
        LevelNumberLabel.Text = level.Number.ToString();
        GridSizeLabel.Text = $"{level.FieldDimensions.Width} × {level.FieldDimensions.Height}";

        System.Diagnostics.Debug.WriteLine($"Level Start - Level: {level.Number}, Grid: {level.FieldDimensions.Width}×{level.FieldDimensions.Height}");
    }

    public Task<bool> WaitForStartAsync()
    {
        return _taskCompletionSource?.Task ?? Task.FromResult(true);
    }

    private void OnStartButtonClicked(object? sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Start button clicked");
        _taskCompletionSource?.TrySetResult(true);
    }

    protected override bool OnBackButtonPressed()
    {
        // Prevent dismissing the dialog with back button
        return true;
    }
}
