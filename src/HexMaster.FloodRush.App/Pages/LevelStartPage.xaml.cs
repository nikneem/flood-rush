using HexMaster.FloodRush.Game.DomainModels;

namespace HexMaster.FloodRush.App.Pages;

public partial class LevelStartPage : ContentPage
{
    public LevelStartPage()
    {
        InitializeComponent();
    }

    public void SetLevelInfo(Level level)
    {
        LevelNumberLabel.Text = level.Number.ToString();
        GridSizeLabel.Text = $"{level.FieldDimensions.Width} × {level.FieldDimensions.Height}";

        System.Diagnostics.Debug.WriteLine($"Level Start - Level: {level.Number}, Grid: {level.FieldDimensions.Width}×{level.FieldDimensions.Height}");
    }

    private async void OnStartButtonClicked(object? sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Start button clicked - closing modal");
        await Navigation.PopModalAsync(animated: false);
    }

    protected override bool OnBackButtonPressed()
    {
        // Prevent dismissing the dialog with back button
        return true;
    }
}
