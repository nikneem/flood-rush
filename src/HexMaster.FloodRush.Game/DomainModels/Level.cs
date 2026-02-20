namespace HexMaster.FloodRush.Game.DomainModels;

public class Level
{
    private readonly List<int> _excludedTileTypes;

    public int Number { get; private set; }
    public int Points { get; private set; }
    public FieldDimensions FieldDimensions { get; private set; }
    public Position StartPosition { get; private set; }
    public Position EndPosition { get; private set; }
    public int GameSpeed { get; private set; }
    public int FloodTimeout { get; private set; }
    public IReadOnlyList<int> ExcludedTileTypes => _excludedTileTypes.AsReadOnly();

    private Level()
    {
        _excludedTileTypes = new List<int>();
        FieldDimensions = FieldDimensions.Create(1, 1);
        StartPosition = Position.Create(0, 0);
        EndPosition = Position.Create(0, 0);
    }

    public static Level Create(
        int number,
        FieldDimensions fieldDimensions,
        Position startPosition,
        Position endPosition,
        int gameSpeed,
        int floodTimeout,
        IEnumerable<int>? excludedTileTypes = null)
    {
        if (number <= 0)
            throw new ArgumentException("Level number must be greater than zero", nameof(number));

        if (gameSpeed < 1 || gameSpeed > 10)
            throw new ArgumentException("Game speed must be between 1 and 10", nameof(gameSpeed));

        if (floodTimeout < 0)
            throw new ArgumentException("Flood timeout cannot be negative", nameof(floodTimeout));

        ValidatePositionWithinBounds(startPosition, fieldDimensions, nameof(startPosition));
        ValidatePositionWithinBounds(endPosition, fieldDimensions, nameof(endPosition));

        var level = new Level
        {
            Number = number,
            Points = 0,
            FieldDimensions = fieldDimensions,
            StartPosition = startPosition,
            EndPosition = endPosition,
            GameSpeed = gameSpeed,
            FloodTimeout = floodTimeout
        };

        if (excludedTileTypes != null)
        {
            level._excludedTileTypes.AddRange(excludedTileTypes);
        }

        return level;
    }

    public void AddPoints(int points)
    {
        if (points < 0)
            throw new ArgumentException("Points to add cannot be negative", nameof(points));

        Points += points;
    }

    public void ResetPoints()
    {
        Points = 0;
    }

    private static void ValidatePositionWithinBounds(Position position, FieldDimensions dimensions, string paramName)
    {
        if (position.X >= dimensions.Width)
            throw new ArgumentException(
                $"Position X ({position.X}) must be less than field width ({dimensions.Width})",
                paramName);

        if (position.Y >= dimensions.Height)
            throw new ArgumentException(
                $"Position Y ({position.Y}) must be less than field height ({dimensions.Height})",
                paramName);
    }
}
