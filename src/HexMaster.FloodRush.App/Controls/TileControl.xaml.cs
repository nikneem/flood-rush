using HexMaster.FloodRush.Game.DomainModels;

namespace HexMaster.FloodRush.App.Controls;

public partial class TileControl : Border
{
    private static readonly Random _random = new();
    private const int TileCount = 12;
    private PipeSection? _placedPipe;

    public static readonly BindableProperty TileNumberProperty =
        BindableProperty.Create(
            nameof(TileNumber),
            typeof(int?),
            typeof(TileControl),
            null,
            propertyChanged: OnTileNumberChanged);

    public int? TileNumber
    {
        get => (int?)GetValue(TileNumberProperty);
        set => SetValue(TileNumberProperty, value);
    }

    public TileControl()
    {
        InitializeComponent();
        LoadRandomTile();
    }

    private static void OnTileNumberChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TileControl control)
        {
            control.LoadTile();
        }
    }

    private void LoadRandomTile()
    {
        if (TileNumber == null)
        {
            TileNumber = _random.Next(1, TileCount + 1);
        }
        else
        {
            LoadTile();
        }
    }

    private void LoadTile()
    {
        var tileNumber = TileNumber ?? _random.Next(1, TileCount + 1);
        TileImage.Source = $"tiles/tile_{tileNumber:D2}.png";
    }
    
    public bool CanAcceptPipe()
    {
        // Tile can accept a pipe if no pipe is currently placed
        return _placedPipe == null;
    }
    
    public void PlacePipe(PipeSection pipeSection)
    {
        _placedPipe = pipeSection;
        PipeOverlay.PipeSection = pipeSection;
        PipeOverlay.IsVisible = true;
        
        System.Diagnostics.Debug.WriteLine($"Pipe placed on tile: {pipeSection.Type}");
    }
    
    public void SetHighlight(bool isHighlighted)
    {
        HighlightOverlay.IsVisible = isHighlighted;
        
        if (isHighlighted)
        {
            // Pulse animation for highlight
            _ = PulseHighlight();
        }
    }
    
    private async Task PulseHighlight()
    {
        while (HighlightOverlay.IsVisible)
        {
            await HighlightOverlay.FadeToAsync(0.6, 500);
            if (HighlightOverlay.IsVisible)
            {
                await HighlightOverlay.FadeToAsync(0.3, 500);
            }
        }
        
        // Reset opacity when hiding
        HighlightOverlay.Opacity = 0.4;
    }
}
