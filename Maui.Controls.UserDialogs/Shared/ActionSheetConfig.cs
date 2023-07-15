namespace Maui.Controls.UserDialogs;

public class ActionSheetConfig
{
    public static float CornerRadius { get; set; } = 15;
    public static Color BackgroundColor { get; set; } = Colors.White;
    public static double TitleFontSize { get; set; } = 20;
    public static Color TitleColor { get; set; } = Colors.Black;
    public static double MessageFontSize { get; set; } = 16;
    public static Color MessageColor { get; set; } = Colors.Black;
    public static Color NegativeButtonTextColor { get; set; } = Colors.Black;
    public static double NegativeButtonFontSize { get; set; } = 18;
    public static Color DestructiveButtonTextColor { get; set; } = Colors.Red;
    public static double DestructiveButtonFontSize { get; set; } = 18;
    public static Color ActionSheetSeparatorColor { get; set; } = Color.FromArgb("#FFE9E9E9");
    public static Color ActionSheetOptionTextColor { get; set; } = Colors.Black;
    public static double ActionSheetOptionFontSize { get; set; } = 18;

    public static string DefaultCancelText { get; set; } = "Cancel";
    public static string DefaultDestructiveText { get; set; } = "Remove";

    public string Title { get; set; }
    public string Message { get; set; }
    public string Icon { get; set; }
    public ActionSheetOption Cancel { get; set; }
    public ActionSheetOption Destructive { get; set; }
    public IList<ActionSheetOption> Options { get; set; } = new List<ActionSheetOption>();

    /// <summary>
    /// This only currently applies to android
    /// </summary>
    public bool UseBottomSheet { get; set; } = true;

    public ActionSheetConfig SetTitle(string title)
    {
        this.Title = title;
        return this;
    }

    public ActionSheetConfig SetIcon(string icon)
    {
        this.Icon = icon;
        return this;
    }

    public ActionSheetConfig SetUseBottomSheet(bool useBottomSheet)
    {
        this.UseBottomSheet = useBottomSheet;
        return this;
    }

    public ActionSheetConfig SetCancel(string text = null, Action action = null, string icon = null)
    {
        this.Cancel = new ActionSheetOption(text ?? DefaultCancelText, action, icon);
        return this;
    }

    public ActionSheetConfig SetDestructive(string text = null, Action action = null, string icon = null)
    {
        this.Destructive = new ActionSheetOption(text ?? DefaultDestructiveText, action, icon);
        return this;
    }

    public ActionSheetConfig SetMessage(string msg)
    {
        this.Message = msg;
        return this;
    }

    public ActionSheetConfig Add(string text, Action action = null, string icon = null)
    {
        this.Options.Add(new ActionSheetOption(text, action, icon));
        return this;
    }
}