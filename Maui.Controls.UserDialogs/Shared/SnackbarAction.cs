namespace Maui.Controls.UserDialogs;

public class SnackbarAction
{
    public string Text { get; set; }
    public string Icon { get; set; }
    public Action Action { get; set; }
    public bool ShowCountDown { get; set; } = true;

    public SnackbarAction SetText(string text)
    {
        this.Text = text;
        return this;
    }

    public SnackbarAction SetTextColor(string icon)
    {
        this.Icon = icon;
        return this;
    }

    public SnackbarAction SetAction(Action action)
    {
        this.Action = action;
        return this;
    }
}