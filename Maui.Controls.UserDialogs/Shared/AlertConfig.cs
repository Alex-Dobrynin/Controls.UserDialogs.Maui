namespace Maui.Controls.UserDialogs;

public class AlertConfig
{
    public static float CornerRadius { get; set; } = 15;
    public static Color BackgroundColor { get; set; } = Colors.White;
    public static double TitleFontSize { get; set; } = 20;
    public static Color TitleColor { get; set; } = Colors.Black;
    public static double MessageFontSize { get; set; } = 16;
    public static Color MessageColor { get; set; } = Colors.Black;
    public static Color PositiveButtonTextColor { get; set; } = Colors.Black;
    public static double PositiveButtonFontSize { get; set; } = 18;

    public static string DefaultOkText { get; set; } = "Ok";

    public string OkText { get; set; } = DefaultOkText;
    public string Title { get; set; }
    public string Message { get; set; }
    public string Icon { get; set; }
    public Action Action { get; set; }

    public AlertConfig SetOkText(string text)
    {
        this.OkText = text;
        return this;
    }

    public AlertConfig SetTitle(string title)
    {
        this.Title = title;
        return this;
    }

    public AlertConfig SetMessage(string message)
    {
        this.Message = message;
        return this;
    }

    public AlertConfig SetAction(Action action)
    {
        this.Action = action;
        return this;
    }

    public AlertConfig SetIcon(string icon)
    {
        this.Icon = icon;
        return this;
    }
}