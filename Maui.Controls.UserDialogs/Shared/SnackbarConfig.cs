namespace Maui.Controls.UserDialogs;

public class SnackbarConfig
{
    public static float CornerRadius { get; set; } = 10;
    public static Color BackgroundColor { get; set; } = Colors.Black.WithAlpha(0.4f);
    public static double MessageFontSize { get; set; } = 16;
    public static Color MessageColor { get; set; } = Colors.White;
    public static Color PositiveButtonTextColor { get; set; } = Colors.White;
    public static double PositiveButtonFontSize { get; set; } = 16;
    public static ToastPosition Position { get; set; }

    public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(10);
    public static bool ShowCountDown { get; set; } = true;

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