using HexMaster.FloodRush.App.ViewModels;
using HexMaster.FloodRush.App.Controls;
using HexMaster.FloodRush.Game.DomainModels;

namespace HexMaster.FloodRush.App.Pages;

public partial class GamePage : ContentPage
{
    private readonly GamePageViewModel _viewModel;
    private System.Timers.Timer? _gameTimer;
    private TimeSpan _elapsedTime;
    private bool _isLevelStarted = false;
    private readonly SemaphoreSlim _initializationLock = new SemaphoreSlim(1, 1);
    private PipeSection? _draggingPipe;
    private PointerGestureRecognizer? _pointerGesture;

    public GamePage(GamePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        SetupDragAndDrop();
    }
    
    private void SetupDragAndDrop()
    {
        // Subscribe to drag events from NextTilesQueue
        NextTilesQueue.PipeDragStarted += OnPipeDragStarted;
        NextTilesQueue.PipeDragEnded += OnPipeDragEnded;
        
        // Setup pointer gesture for tracking drag movement
        _pointerGesture = new PointerGestureRecognizer();
        _pointerGesture.PointerMoved += OnPointerMoved;
        ContentGrid.GestureRecognizers.Add(_pointerGesture);
        
        // Setup drop gesture for playfield
        var dropGesture = new DropGestureRecognizer();
        dropGesture.Drop += OnDrop;
        PlayField.GestureRecognizers.Add(dropGesture);
    }
    
    private void OnPipeDragStarted(object? sender, PipeDragStartedEventArgs e)
    {
        _draggingPipe = e.PipeSection;
        
        // Show and position the drag overlay
        DragOverlay.PipeSection = e.PipeSection;
        DragOverlay.IsVisible = true;
        DragOverlay.Opacity = 0.8;
        
        // Position at touch point (centered on the pipe)
        AbsoluteLayout.SetLayoutBounds(DragOverlay, new Rect(e.StartPosition.X - 20, e.StartPosition.Y - 20, 40, 40));
        
        System.Diagnostics.Debug.WriteLine($"Drag overlay shown at {e.StartPosition}");
    }
    
    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_draggingPipe != null && DragOverlay.IsVisible)
        {
            var position = e.GetPosition(this);
            if (position.HasValue)
            {
                // Update drag overlay position (centered on pointer)
                AbsoluteLayout.SetLayoutBounds(DragOverlay, new Rect(position.Value.X - 20, position.Value.Y - 20, 40, 40));
                
                // Check if hovering over a valid tile and highlight it
                PlayField.CheckHoverPosition(position.Value);
            }
        }
    }
    
    private void OnDrop(object? sender, DropEventArgs e)
    {
        if (_draggingPipe != null)
        {
            var position = e.GetPosition(PlayField);
            if (position.HasValue)
            {
                var success = PlayField.TryPlacePipe(_draggingPipe, position.Value);
                NextTilesQueue.OnDragCompleted(success);
                
                System.Diagnostics.Debug.WriteLine($"Drop at {position.Value}, success: {success}");
            }
            else
            {
                NextTilesQueue.OnDragCompleted(false);
            }
        }
        
        // Hide drag overlay
        DragOverlay.IsVisible = false;
        _draggingPipe = null;
    }
    
    private void OnPipeDragEnded(object? sender, EventArgs e)
    {
        // Hide drag overlay if drag ended without drop
        DragOverlay.IsVisible = false;
        _draggingPipe = null;
        PlayField.ClearHighlight();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Prevent concurrent initialization attempts
        if (!await _initializationLock.WaitAsync(0))
        {
            return; // Already initializing
        }

        try
        {
            // Only show the level start dialog and initialize once
            if (!_isLevelStarted && _viewModel.Level != null)
            {
                _isLevelStarted = true;

                PlayField.InitializeLevel(_viewModel.Level);

                // Show level start dialog
                var levelStartPage = new LevelStartPage();
                levelStartPage.SetLevelInfo(_viewModel.Level);

                // Show modal and set up continuation when it's dismissed
                await Navigation.PushModalAsync(levelStartPage, animated: false);

                // Subscribe to the Disappearing event to know when modal is closed
                var tcs = new TaskCompletionSource<bool>();
                void OnDisappearing(object? s, EventArgs e)
                {
                    levelStartPage.Disappearing -= OnDisappearing;
                    tcs.TrySetResult(true);
                }
                levelStartPage.Disappearing += OnDisappearing;

                // Wait for the modal to be closed (by the Start button)
                await tcs.Task;

                // Load tiles progressively into the queue
                await NextTilesQueue.LoadTilesProgressivelyAsync();

                StartGameTimer();
            }
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        StopGameTimer();
        _isLevelStarted = false;
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
