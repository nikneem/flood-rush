using HexMaster.FloodRush.Game.DomainModels;

namespace HexMaster.FloodRush.App.Controls;

public partial class PlayFieldControl : ContentView
{
    private Point _lastPanPoint;
    private bool _isPanning;

    public PlayFieldControl()
    {
        InitializeComponent();
        SetupGestureRecognizers();
    }

    private void SetupGestureRecognizers()
    {
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        GestureRecognizers.Add(panGesture);
    }

    private async void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _isPanning = true;
                _lastPanPoint = new Point(e.TotalX, e.TotalY);
                break;

            case GestureStatus.Running:
                if (_isPanning)
                {
                    var deltaX = e.TotalX - _lastPanPoint.X;
                    var deltaY = e.TotalY - _lastPanPoint.Y;

                    // Scroll in opposite direction of pan (natural scrolling)
                    var newScrollX = ScrollContainer.ScrollX - deltaX;
                    var newScrollY = ScrollContainer.ScrollY - deltaY;

                    // Clamp to valid scroll range
                    newScrollX = Math.Max(0, Math.Min(newScrollX, ScrollContainer.ContentSize.Width - ScrollContainer.Width));
                    newScrollY = Math.Max(0, Math.Min(newScrollY, ScrollContainer.ContentSize.Height - ScrollContainer.Height));

                    await ScrollContainer.ScrollToAsync(newScrollX, newScrollY, false);

                    _lastPanPoint = new Point(e.TotalX, e.TotalY);
                }
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                _isPanning = false;
                break;
        }
    }

    /// <summary>
    /// Gets the content container grid where child views should be added
    /// </summary>
    public Grid ContentGrid => ContentContainer;

    /// <summary>
    /// Initializes the playfield with the given level, creating a grid of tiles
    /// </summary>
    public void InitializeLevel(Level level)
    {
        // Clear existing content
        ContentContainer.Children.Clear();
        ContentContainer.RowDefinitions.Clear();
        ContentContainer.ColumnDefinitions.Clear();

        var dimensions = level.FieldDimensions;

        // Create row definitions
        for (int row = 0; row < dimensions.Height; row++)
        {
            ContentContainer.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        }

        // Create column definitions
        for (int col = 0; col < dimensions.Width; col++)
        {
            ContentContainer.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        }

        // Add tile controls to the grid
        for (int row = 0; row < dimensions.Height; row++)
        {
            for (int col = 0; col < dimensions.Width; col++)
            {
                var tileControl = CreateTileControl();
                Grid.SetRow(tileControl, row);
                Grid.SetColumn(tileControl, col);
                ContentContainer.Children.Add(tileControl);
            }
        }
    }

    private TileControl CreateTileControl()
    {
        // Create a new TileControl which will automatically load a random tile
        return new TileControl();
    }
    
    private TileControl? _highlightedTile;
    
    public void CheckHoverPosition(Point position)
    {
        // Get the position relative to the content container
        var localPosition = position;
        
        // Find which tile is at this position
        var tile = GetTileAtPosition(localPosition);
        
        if (tile != _highlightedTile)
        {
            // Clear previous highlight
            if (_highlightedTile != null)
            {
                _highlightedTile.SetHighlight(false);
            }
            
            // Highlight new tile if it can accept a pipe
            if (tile != null && tile.CanAcceptPipe())
            {
                tile.SetHighlight(true);
                _highlightedTile = tile;
            }
            else
            {
                _highlightedTile = null;
            }
        }
    }
    
    public bool TryPlacePipe(PipeSection pipeSection, Point position)
    {
        var tile = GetTileAtPosition(position);
        
        if (tile != null && tile.CanAcceptPipe())
        {
            tile.PlacePipe(pipeSection);
            ClearHighlight();
            return true;
        }
        
        return false;
    }
    
    public void ClearHighlight()
    {
        if (_highlightedTile != null)
        {
            _highlightedTile.SetHighlight(false);
            _highlightedTile = null;
        }
    }
    
    private TileControl? GetTileAtPosition(Point position)
    {
        foreach (var child in ContentContainer.Children)
        {
            if (child is TileControl tile)
            {
                var bounds = tile.Bounds;
                var tilePosition = GetAbsolutePosition(tile);
                
                var tileBounds = new Rect(tilePosition.X, tilePosition.Y, bounds.Width, bounds.Height);
                
                if (tileBounds.Contains(position))
                {
                    return tile;
                }
            }
        }
        
        return null;
    }
    
    private Point GetAbsolutePosition(View view)
    {
        var x = 0.0;
        var y = 0.0;
        
        var element = view as IView;
        while (element != null && element != this)
        {
            x += element.Frame.X;
            y += element.Frame.Y;
            element = element.Parent as IView;
        }
        
        return new Point(x, y);
    }
}
