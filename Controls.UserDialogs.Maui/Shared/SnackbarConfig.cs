namespace Controls.UserDialogs.Maui;

public class SnackbarConfig
{
    public static string DefaultMessageFontFamily { get; set; }
    public static string DefaultNegativeButtonFontFamily { get; set; }

    public static float DefaultCornerRadius { get; set; } = 10;
    public static Color DefaultBackgroundColor { get; set; }
    public static double DefaultMessageFontSize { get; set; } = 16;
    public static Color DefaultMessageColor { get; set; }
    public static Color DefaultNegativeButtonTextColor { get; set; }
    public static double DefaultNegativeButtonFontSize { get; set; } = 16;
    public static ToastPosition DefaultPosition { get; set; }

    public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(10);

    public string MessageFontFamily { get; set; } = DefaultMessageFontFamily;
    public string NegativeButtonFontFamily { get; set; } = DefaultNegativeButtonFontFamily;

    public Color BackgroundColor { get; set; } = DefaultBackgroundColor;
    /// <summary>
    /// Android only
    /// </summary>
    /// <remarks>
    /// Works only if <see cref="BackgroundColor"/> was set
    /// </remarks>
    public float CornerRadius { get; set; } = DefaultCornerRadius;
    public double MessageFontSize { get; set; } = DefaultMessageFontSize;
    public Color MessageColor { get; set; } = DefaultMessageColor;
    public Color NegativeButtonTextColor { get; set; } = DefaultNegativeButtonTextColor;
    public double NegativeButtonFontSize { get; set; } = DefaultNegativeButtonFontSize;
    public ToastPosition Position { get; set; } = DefaultPosition;

    public string Message { get; set; }
    public string Icon { get; set; }
    public TimeSpan Duration { get; set; } = DefaultDuration;
    public Action<SnackbarActionType> Action { get; set; }
    public string ActionText { get; set; }
    public string ActionIcon { get; set; }
    public bool ShowCountDown { get; set; }

    public SnackbarConfig SetMessage(string message)
    {
        this.Message = message;
        return this;
    }

    public SnackbarConfig SetIcon(string icon)
    {
        this.Icon = icon;
        return this;
    }

    public SnackbarConfig SetAction(Action<SnackbarActionType> action)
    {
        this.Action = action;
        return this;
    }

    public SnackbarConfig SetDuration(int millis) => this.SetDuration(TimeSpan.FromMilliseconds(millis));

    public SnackbarConfig SetDuration(TimeSpan? duration = null)
    {
        this.Duration = duration ?? DefaultDuration;
        return this;
    }
}