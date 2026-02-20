namespace HexMaster.FloodRush.Game.DomainModels;

public record FieldDimensions(int Width, int Height)
{
    public static FieldDimensions Create(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentException("Width must be greater than zero", nameof(width));
        if (height <= 0)
            throw new ArgumentException("Height must be greater than zero", nameof(height));

        return new FieldDimensions(width, height);
    }
}
