using HexMaster.FloodRush.Game.DomainModels;

namespace HexMaster.FloodRush.App.Controls;

public partial class NextTilesQueueControl : Border
{
    private const int QueueSize = 20;
    private const uint AnimationDuration = 400;
    private const uint BounceBackDuration = 100;
    private readonly List<PipeSection> _pipeQueue = new();
    private readonly Random _random = new();

    public NextTilesQueueControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Progressively loads tiles into the queue with a 200ms delay between each tile
    /// until the queue contains 20 tiles.
    /// </summary>
    public async Task LoadTilesProgressivelyAsync()
    {
        // Clear any existing queue
        _pipeQueue.Clear();
        TilesContainer.Children.Clear();

        // Add tiles progressively with 200ms interval
        for (int i = 0; i < QueueSize; i++)
        {
            var newPipe = GenerateRandomPipeSection(i);
            _pipeQueue.Add(newPipe);

            // Create and add the pipe control positioned off-screen to the right
            var pipeControl = new PipeControl
            {
                PipeSection = newPipe,
                TranslationX = 200, // Start off-screen to the right
                Opacity = 0
            };

            TilesContainer.Children.Add(pipeControl);

            // Slide in from right with fade-in, creating a gravity effect
            await Task.WhenAll(
                pipeControl.TranslateToAsync(5, 0, 300, Easing.CubicOut),
                pipeControl.FadeToAsync(1, 300)
            );

            // Bounce back to final position for gravity effect
            await pipeControl.TranslateToAsync(0, 0, BounceBackDuration, Easing.BounceOut);

            // Wait 200ms before adding the next tile (except for the last one)
            if (i < QueueSize - 1)
            {
                await Task.Delay(50);
            }
        }
    }

    private PipeSection GenerateRandomPipeSection(int index)
    {
        var types = new[] { PipeType.Straight, PipeType.Corner, PipeType.CrossSection };
        var type = types[_random.Next(types.Length)];

        var (entry, exit, secondaryPoints) = type switch
        {
            PipeType.Straight => GenerateStraightPipe(),
            PipeType.Corner => GenerateCornerPipe(),
            PipeType.CrossSection => GenerateCrossSectionPipe(),
            _ => GenerateStraightPipe()
        };

        return PipeSection.Create(
            Position.Create(0, 0), // Position doesn't matter for queue display
            type,
            entry,
            exit,
            speedMultiplier: 0.5f + ((float)_random.NextDouble() * 0.5f),
            points: _random.Next(10, 100),
            secondaryPoints: secondaryPoints
        );
    }

    private (PipeConnection entry, PipeConnection exit, int? secondaryPoints) GenerateStraightPipe()
    {
        var isHorizontal = _random.Next(2) == 0;
        if (isHorizontal)
        {
            return (PipeConnection.West, PipeConnection.East, null);
        }
        return (PipeConnection.North, PipeConnection.South, null);
    }

    private (PipeConnection entry, PipeConnection exit, int? secondaryPoints) GenerateCornerPipe()
    {
        var corners = new[]
        {
            (PipeConnection.North, PipeConnection.East),
            (PipeConnection.North, PipeConnection.West),
            (PipeConnection.South, PipeConnection.East),
            (PipeConnection.South, PipeConnection.West)
        };

        var corner = corners[_random.Next(corners.Length)];
        return (corner.Item1, corner.Item2, null);
    }

    private (PipeConnection entry, PipeConnection exit, int? secondaryPoints) GenerateCrossSectionPipe()
    {
        // Cross section has both horizontal and vertical flows
        return (PipeConnection.North, PipeConnection.South, _random.Next(10, 100));
    }

    public async Task<PipeSection> TakeNextPipeAsync()
    {
        if (_pipeQueue.Count == 0)
            throw new InvalidOperationException("Queue is empty");

        // Get the first (leftmost) pipe
        var pipe = _pipeQueue[0];
        _pipeQueue.RemoveAt(0);

        // Animate remaining pipes sliding left
        await AnimateSlidingLeft();

        // Add new pipe to the end with slide-in animation
        var newPipe = GenerateRandomPipeSection(_pipeQueue.Count);
        _pipeQueue.Add(newPipe);
        await AnimateNewPipeEntry(newPipe);

        return pipe;
    }

    private async Task AnimateSlidingLeft()
    {
        var tasks = new List<Task>();

        for (int i = 0; i < TilesContainer.Children.Count; i++)
        {
            if (TilesContainer.Children[i] is PipeControl control)
            {
                // Slide left with gravity effect
                tasks.Add(AnimateSlideWithBounce(control));
            }
        }

        // Remove the first control after animation
        if (TilesContainer.Children.Count > 0)
        {
            TilesContainer.Children.RemoveAt(0);
        }

        await Task.WhenAll(tasks);
    }

    private async Task AnimateSlideWithBounce(View view)
    {
        var originalX = view.TranslationX;
        var targetX = originalX - 48; // Width of pipe (40) + spacing (8)

        // Slide to target position
        await view.TranslateToAsync(targetX - 5, 0, AnimationDuration, Easing.CubicOut);

        // Bounce back slightly
        await view.TranslateToAsync(targetX, 0, BounceBackDuration, Easing.BounceOut);

        // Reset translation for proper layout
        view.TranslationX = 0;
    }

    private async Task AnimateNewPipeEntry(PipeSection newPipe)
    {
        var pipeControl = new PipeControl
        {
            PipeSection = newPipe,
            TranslationX = 200, // Start off-screen to the right
            Opacity = 0
        };

        TilesContainer.Children.Add(pipeControl);

        // Fade in and slide in from right
        await Task.WhenAll(
            pipeControl.TranslateToAsync(5, 0, AnimationDuration, Easing.CubicOut),
            pipeControl.FadeToAsync(1, AnimationDuration)
        );

        // Bounce back to final position
        await pipeControl.TranslateToAsync(0, 0, BounceBackDuration, Easing.BounceOut);
    }

    public PipeSection PeekNext()
    {
        if (_pipeQueue.Count == 0)
            throw new InvalidOperationException("Queue is empty");

        return _pipeQueue[0];
    }

    public int RemainingCount => _pipeQueue.Count;
}
