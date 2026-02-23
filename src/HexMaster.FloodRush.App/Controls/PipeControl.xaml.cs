using HexMaster.FloodRush.Game.DomainModels;

namespace HexMaster.FloodRush.App.Controls;

public partial class PipeControl : Border
{
    public static readonly BindableProperty PipeSectionProperty =
        BindableProperty.Create(
            nameof(PipeSection),
            typeof(PipeSection),
            typeof(PipeControl),
            null,
            propertyChanged: OnPipeSectionChanged);

    public PipeSection? PipeSection
    {
        get => (PipeSection?)GetValue(PipeSectionProperty);
        set => SetValue(PipeSectionProperty, value);
    }

    public PipeControl()
    {
        InitializeComponent();
    }

    private static void OnPipeSectionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PipeControl control && newValue is PipeSection pipeSection)
        {
            control.UpdatePipeDisplay(pipeSection);
        }
    }

    private void UpdatePipeDisplay(PipeSection pipeSection)
    {
        var imageName = GetPipeImageName(pipeSection);
        PipeImage.Source = ImageSource.FromFile(imageName);

        // Update visual state based on pipe state
        UpdatePipeState(pipeSection);
    }

    private string GetPipeImageName(PipeSection pipeSection)
    {
        // Determine the image based on pipe type and connections
        var baseFolder = "pipes/";

        return pipeSection.Type switch
        {
            PipeType.Straight => GetStraightPipeImage(pipeSection, baseFolder),
            PipeType.Corner => GetCornerPipeImage(pipeSection, baseFolder),
            PipeType.CrossSection => $"{baseFolder}cross_section.png",
            _ => $"{baseFolder}horizontal_section.png" // Default fallback
        };
    }

    private string GetStraightPipeImage(PipeSection pipeSection, string baseFolder)
    {
        // Straight pipes can be horizontal (East-West) or vertical (North-South)
        var isHorizontal = (pipeSection.EntryPoint == PipeConnection.East && pipeSection.ExitPoint == PipeConnection.West) ||
                          (pipeSection.EntryPoint == PipeConnection.West && pipeSection.ExitPoint == PipeConnection.East);

        return isHorizontal ? $"{baseFolder}horizontal_section.png" : $"{baseFolder}vertical_section.png";
    }

    private string GetCornerPipeImage(PipeSection pipeSection, string baseFolder)
    {
        // Determine corner section number based on entry and exit points
        // Corner 1: left to bottom (West to South)
        // Corner 2: left to top (West to North)
        // Corner 3: right to top (East to North)
        // Corner 4: right to bottom (East to South)

        var sectionNumber = GetCornerSectionNumber(pipeSection.EntryPoint, pipeSection.ExitPoint);
        return $"{baseFolder}corner_section_{sectionNumber}.png";
    }

    private int GetCornerSectionNumber(PipeConnection entry, PipeConnection exit)
    {
        // Check all combinations (order doesn't matter for corners)
        var connections = new HashSet<PipeConnection> { entry, exit };

        // Corner 1: West to South (left to bottom)
        if (connections.Contains(PipeConnection.West) && connections.Contains(PipeConnection.South))
            return 1;

        // Corner 2: West to North (left to top)
        if (connections.Contains(PipeConnection.West) && connections.Contains(PipeConnection.North))
            return 2;

        // Corner 3: East to North (right to top)
        if (connections.Contains(PipeConnection.East) && connections.Contains(PipeConnection.North))
            return 3;

        // Corner 4: East to South (right to bottom)
        if (connections.Contains(PipeConnection.East) && connections.Contains(PipeConnection.South))
            return 4;

        // Default fallback
        return 1;
    }

    private void UpdatePipeState(PipeSection pipeSection)
    {
        // Update opacity or color based on state
        switch (pipeSection.State)
        {
            case PipeState.Idle:
                this.Opacity = 0.5;
                break;
            case PipeState.Placed:
                this.Opacity = 0.8;
                break;
            case PipeState.Connected:
                this.Opacity = 1.0;
                break;
            case PipeState.Flowing:
                this.Opacity = 1.0;
                FlowIndicator.IsVisible = true;
                // TODO: Add animation for flowing water
                break;
            case PipeState.Full:
                this.Opacity = 1.0;
                FlowIndicator.IsVisible = false;
                // TODO: Show filled pipe visual
                break;
        }
    }
}
