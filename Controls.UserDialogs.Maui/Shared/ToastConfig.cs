namespace Controls.UserDialogs.Maui;

public class ToastConfig
{
    public static string DefaultFontFamily { get; set; }
    public static float DefaultCornerRadius { get; set; } = 15;
    public static Color DefaultBackgroundColor { get; set; }
    public static double DefaultMessageFontSize { get; set; } = 16;
    public static Color DefaultMessageColor { get; set; }
    public static ToastPosition DefaultPosition { get; set; }

    public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(3);

    public string Message { get; set; }
    public string Icon { get; set; }
    public Color MessageColor { get; set; } = DefaultMessageColor;
    public Color BackgroundColor { get; set; } = DefaultBackgroundColor;
    public double MessageFontSize { get; set; } = DefaultMessageFontSize;
    /// <summary>
    /// Android only
    /// </summary>
    /// <remarks>
    /// Works only if <see cref="BackgroundColor"/> was set
    /// </remarks>
    public float CornerRadius { get; set; } = 15;
    public ToastPosition Position { get; set; } = DefaultPosition;
    public TimeSpan Duration { get; set; } = DefaultDuration;
    public string FontFamily { get; set; } = DefaultFontFamily;

    public ToastConfig SetMessage(string message)
    {
        this.Message = message;
        return this;
    }

    public ToastConfig SetIcon(string icon)
    {
        this.Icon = icon;
        return this;
    }

    public ToastConfig SetDuration(int millis) => this.SetDuration(TimeSpan.FromMilliseconds(millis));

    public ToastConfig SetDuration(TimeSpan? duration = null)
    {
        this.Duration = duration ?? DefaultDuration;
        return this;
    }
}