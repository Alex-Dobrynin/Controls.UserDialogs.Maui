namespace Maui.Controls.UserDialogs;

public class ToastConfig
{
    public static float CornerRadius { get; set; } = 15;
    public static Color BackgroundColor { get; set; } = Colors.Black.WithAlpha(0.4f);
    public static double MessageFontSize { get; set; } = 16;
    public static Color MessageColor { get; set; } = Colors.White;
    public static ToastPosition Position { get; set; }

    public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(2.5);

    public string Message { get; set; }
    public string Icon { get; set; }
    //public Color MessageTextColor { get; set; } = DefaultMessageTextColor;
    //public Color BackgroundColor { get; set; } = DefaultBackgroundColor;
    //public ToastPosition Position { get; set; } = DefaultPosition;
    public TimeSpan Duration { get; set; } = DefaultDuration;
    //public ToastAction Action { get; set; }
    //public string Icon { get; set; }

    public ToastConfig()
    {

    }

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

    //public ToastConfig SetBackgroundColor(Color color)
    //{
    //    this.BackgroundColor = color;
    //    return this;
    //}

    //public ToastConfig SetPosition(ToastPosition position)
    //{
    //    this.Position = position;
    //    return this;
    //}

    public ToastConfig SetDuration(int millis) => this.SetDuration(TimeSpan.FromMilliseconds(millis));

    public ToastConfig SetDuration(TimeSpan? duration = null)
    {
        this.Duration = duration ?? DefaultDuration;
        return this;
    }

    //public ToastConfig SetAction(Action<ToastAction> action)
    //{
    //    var cfg = new ToastAction();
    //    action(cfg);
    //    return this.SetAction(cfg);
    //}

    //public ToastConfig SetAction(ToastAction action)
    //{
    //    this.Action = action;
    //    action.TextColor ??= DefaultActionTextColor;

    //    return this;
    //}

    //public ToastConfig SetMessageTextColor(Color color)
    //{
    //    this.MessageTextColor = color;
    //    return this;
    //}

    //public ToastConfig SetIcon(string icon)
    //{
    //    this.Icon = icon;
    //    return this;
    //}
}