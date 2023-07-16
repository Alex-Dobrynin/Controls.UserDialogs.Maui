namespace Maui.Controls.UserDialogs;

public class SnackbarConfig
{
    public static string DefaultFontFamily { get; set; }

    public static float DefaultCornerRadius { get; set; } = 10;
    public static Color DefaultBackgroundColor { get; set; }
    public static double DefaultMessageFontSize { get; set; } = 16;
    public static Color DefaultMessageColor { get; set; }
    public static Color DefaultPositiveButtonTextColor { get; set; }
    public static double DefaultPositiveButtonFontSize { get; set; } = 16;
    public static ToastPosition DefaultPosition { get; set; }

    public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(10);

    public string FontFamily { get; set; } = DefaultFontFamily;
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
    public Color PositiveButtonTextColor { get; set; } = DefaultPositiveButtonTextColor;
    public double PositiveButtonFontSize { get; set; } = DefaultPositiveButtonFontSize;
    public ToastPosition Position { get; set; } = DefaultPosition;

    public string Message { get; set; }
    public string Icon { get; set; }
    public TimeSpan Duration { get; set; } = DefaultDuration;
    public SnackbarAction Action { get; set; }

    public SnackbarConfig()
    {

    }

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

    public SnackbarConfig SetDuration(int millis) => this.SetDuration(TimeSpan.FromMilliseconds(millis));

    public SnackbarConfig SetDuration(TimeSpan? duration = null)
    {
        this.Duration = duration ?? DefaultDuration;
        return this;
    }

    public SnackbarConfig SetAction(SnackbarAction action)
    {
        this.Action = action;

        return this;
    }
}