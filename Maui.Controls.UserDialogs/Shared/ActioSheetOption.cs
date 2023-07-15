namespace Maui.Controls.UserDialogs;

public class ActionSheetOption
{
    public string Text { get; set; }
    public Action Action { get; set; }
    public string Icon { get; set; }

    public ActionSheetOption(string text, Action action = null, string icon = null)
    {
        this.Text = text;
        this.Action = action;
        this.Icon = icon;
    }
}