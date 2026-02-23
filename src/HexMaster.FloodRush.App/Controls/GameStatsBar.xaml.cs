namespace HexMaster.FloodRush.App.Controls;

public partial class GameStatsBar : Border
{
    public static readonly BindableProperty CurrentLevelProperty =
        BindableProperty.Create(
            nameof(CurrentLevel),
            typeof(int),
            typeof(GameStatsBar),
            0,
            propertyChanged: OnCurrentLevelChanged);

    public static readonly BindableProperty TimeProperty =
        BindableProperty.Create(
            nameof(Time),
            typeof(string),
            typeof(GameStatsBar),
            "00:00",
            propertyChanged: OnTimeChanged);

    public static readonly BindableProperty HighScoreProperty =
        BindableProperty.Create(
            nameof(HighScore),
            typeof(int),
            typeof(GameStatsBar),
            0,
            propertyChanged: OnHighScoreChanged);

    public static readonly BindableProperty ScoreProperty =
        BindableProperty.Create(
            nameof(Score),
            typeof(int),
            typeof(GameStatsBar),
            0,
            propertyChanged: OnScoreChanged);

    public int CurrentLevel
    {
        get => (int)GetValue(CurrentLevelProperty);
        set => SetValue(CurrentLevelProperty, value);
    }

    public string Time
    {
        get => (string)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public int HighScore
    {
        get => (int)GetValue(HighScoreProperty);
        set => SetValue(HighScoreProperty, value);
    }

    public int Score
    {
        get => (int)GetValue(ScoreProperty);
        set => SetValue(ScoreProperty, value);
    }

    public GameStatsBar()
    {
        InitializeComponent();
        UpdateLabels();
    }

    private static void OnCurrentLevelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is GameStatsBar control)
        {
            control.LevelLabel.Text = newValue.ToString();
        }
    }

    private static void OnTimeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is GameStatsBar control)
        {
            control.TimeLabel.Text = newValue?.ToString() ?? "00:00";
        }
    }

    private static void OnHighScoreChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is GameStatsBar control)
        {
            control.HighScoreLabel.Text = newValue.ToString();
        }
    }

    private static void OnScoreChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is GameStatsBar control)
        {
            control.ScoreLabel.Text = newValue.ToString();
        }
    }

    private void UpdateLabels()
    {
        LevelLabel.Text = CurrentLevel.ToString();
        TimeLabel.Text = Time;
        HighScoreLabel.Text = HighScore.ToString();
        ScoreLabel.Text = Score.ToString();
    }
}
