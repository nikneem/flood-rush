namespace HexMaster.FloodRush.App.Controls;

public partial class TileControl : Border
{
    private static readonly Random _random = new();
    private const int TileCount = 12;

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
}
