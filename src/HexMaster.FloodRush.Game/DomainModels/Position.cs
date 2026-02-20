namespace HexMaster.FloodRush.Game.DomainModels;

public record Position(int X, int Y)
{
    public static Position Create(int x, int y)
    {
        if (x < 0)
            throw new ArgumentException("X coordinate cannot be negative", nameof(x));
        if (y < 0)
            throw new ArgumentException("Y coordinate cannot be negative", nameof(y));

        return new Position(x, y);
    }
}
