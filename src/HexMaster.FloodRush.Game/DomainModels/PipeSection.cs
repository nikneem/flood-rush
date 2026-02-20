namespace HexMaster.FloodRush.Game.DomainModels;

public class PipeSection
{
    private int _flowCount;

    public Position Coordinate { get; private set; }
    public PipeType Type { get; private set; }
    public PipeConnection EntryPoint { get; private set; }
    public PipeConnection ExitPoint { get; private set; }
    public FlowDirection Direction { get; private set; }
    public float SpeedMultiplier { get; private set; }
    public int Points { get; private set; }
    public int? SecondaryPoints { get; private set; }
    public PipeState State { get; private set; }

    public bool CanBeRemoved => State is PipeState.Placed or PipeState.Connected;

    public int MaxFlowCount => Type == PipeType.CrossSection ? 2 : 1;

    public bool CanAcceptFlow => _flowCount < MaxFlowCount;

    private PipeSection()
    {
        Coordinate = Position.Create(0, 0);
        State = PipeState.Idle;
        Direction = FlowDirection.Default;
        _flowCount = 0;
    }

    public static PipeSection Create(
        Position coordinate,
        PipeType type,
        PipeConnection entryPoint,
        PipeConnection exitPoint,
        float speedMultiplier,
        int points,
        int? secondaryPoints = null)
    {
        if (speedMultiplier < 0 || speedMultiplier > 1)
            throw new ArgumentException("Speed multiplier must be between 0 and 1", nameof(speedMultiplier));

        if (points < 0)
            throw new ArgumentException("Points cannot be negative", nameof(points));

        if (type == PipeType.CrossSection)
        {
            if (!secondaryPoints.HasValue)
                throw new ArgumentException("Cross section pipes require secondary points", nameof(secondaryPoints));

            if (secondaryPoints.Value < 0)
                throw new ArgumentException("Secondary points cannot be negative", nameof(secondaryPoints));
        }

        ValidatePipeConnections(type, entryPoint, exitPoint);

        var pipe = new PipeSection
        {
            Coordinate = coordinate,
            Type = type,
            EntryPoint = entryPoint,
            ExitPoint = exitPoint,
            Direction = FlowDirection.Default,
            SpeedMultiplier = speedMultiplier,
            Points = points,
            SecondaryPoints = secondaryPoints,
            State = PipeState.Idle
        };

        return pipe;
    }

    public void Place()
    {
        if (State != PipeState.Idle)
            throw new InvalidOperationException($"Cannot place pipe in {State} state");

        State = PipeState.Placed;
    }

    public void Connect()
    {
        if (State != PipeState.Placed)
            throw new InvalidOperationException($"Cannot connect pipe in {State} state");

        State = PipeState.Connected;
    }

    public void StartFlow()
    {
        if (State != PipeState.Connected)
            throw new InvalidOperationException($"Cannot start flow in {State} state");

        State = PipeState.Flowing;
        _flowCount++;
    }

    public void CompleteFill()
    {
        if (State != PipeState.Flowing)
            throw new InvalidOperationException($"Cannot complete fill in {State} state");

        State = PipeState.Full;
    }

    public void ReverseDirection()
    {
        if (State is PipeState.Flowing or PipeState.Full)
            throw new InvalidOperationException("Cannot reverse direction while water is flowing or pipe is full");

        Direction = Direction == FlowDirection.Default
            ? FlowDirection.Reverse
            : FlowDirection.Default;
    }

    public PipeConnection GetActualEntryPoint()
    {
        return Direction == FlowDirection.Default ? EntryPoint : ExitPoint;
    }

    public PipeConnection GetActualExitPoint()
    {
        return Direction == FlowDirection.Default ? ExitPoint : EntryPoint;
    }

    public int GetPointsForCurrentFlow()
    {
        if (Type != PipeType.CrossSection)
            return Points;

        return _flowCount == 1 ? Points : (SecondaryPoints ?? 0);
    }

    public void Reset()
    {
        State = PipeState.Idle;
        Direction = FlowDirection.Default;
        _flowCount = 0;
    }

    private static void ValidatePipeConnections(PipeType type, PipeConnection entry, PipeConnection exit)
    {
        if (entry == exit)
            throw new ArgumentException("Entry and exit points cannot be the same");

        switch (type)
        {
            case PipeType.Straight:
                ValidateStraightPipe(entry, exit);
                break;
            case PipeType.Corner:
                ValidateCornerPipe(entry, exit);
                break;
            case PipeType.CrossSection:
                // Cross sections can have any valid combination
                break;
        }
    }

    private static void ValidateStraightPipe(PipeConnection entry, PipeConnection exit)
    {
        var validCombinations = new[]
        {
            (PipeConnection.North, PipeConnection.South),
            (PipeConnection.South, PipeConnection.North),
            (PipeConnection.East, PipeConnection.West),
            (PipeConnection.West, PipeConnection.East)
        };

        if (!validCombinations.Contains((entry, exit)))
            throw new ArgumentException("Invalid straight pipe connection combination");
    }

    private static void ValidateCornerPipe(PipeConnection entry, PipeConnection exit)
    {
        var validCombinations = new[]
        {
            (PipeConnection.North, PipeConnection.East),
            (PipeConnection.North, PipeConnection.West),
            (PipeConnection.South, PipeConnection.East),
            (PipeConnection.South, PipeConnection.West),
            (PipeConnection.East, PipeConnection.North),
            (PipeConnection.East, PipeConnection.South),
            (PipeConnection.West, PipeConnection.North),
            (PipeConnection.West, PipeConnection.South)
        };

        if (!validCombinations.Contains((entry, exit)))
            throw new ArgumentException("Invalid corner pipe connection combination");
    }
}
