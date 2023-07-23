namespace Controls.UserDialogs.Maui;

public class ActionSheetConfig
{
    public static string DefaultTitleFontFamily { get; set; }
    public static string DefaultFontFamily { get; set; }
    /// <summary>
    /// iOS only
    /// </summary>
    public static UserInterfaceStyle? DefaultUserInterfaceStyle { get; set; }

    /// <summary>
    /// Android only
    /// </summary>
    public static Color DefaultBackgroundColor { get; set; }

    /// <summary>
    /// Android only
    /// </summary>
    public static float DefaultCornerRadius { get; set; } = 15;

    public static Color DefaultMessageColor { get; set; }
    public static Color DefaultTitleColor { get; set; }
    public static float DefaultTitleFontSize { get; set; } = 20;
    public static float DefaultMessageFontSize { get; set; } = 14;
    public static Color DefaultDestructiveButtonTextColor { get; set; } = Colors.Red;

    /// <summary>
    /// Android only
    /// </summary>
    public static float DefaultDestructiveButtonFontSize { get; set; } = 16;
    public static Color DefaultNegativeButtonTextColor { get; set; }

    /// <summary>
    /// Android only
    /// </summary>
    public static float DefaultNegativeButtonFontSize { get; set; } = 18;
    public static Color DefaultActionSheetOptionTextColor { get; set; }

    /// <summary>
    /// Android only
    /// </summary>
    public static float DefaultActionSheetOptionFontSize { get; set; } = 16;

    public string TitleFontFamily { get; set; } = DefaultTitleFontFamily;
    public string FontFamily { get; set; } = DefaultFontFamily;

    /// <summary>
    /// Android only
    /// </summary>
    public static Color DefaultActionSheetSeparatorColor { get; set; } = Color.FromArgb("#FFE9E9E9");

    /// <summary>
    /// iOS only
    /// </summary>
    /// <remarks>
    /// Used to set light or dark mode for dialog, if not set, the system theme will be used
    /// </remarks>
    public UserInterfaceStyle? UserInterfaceStyle { get; set; } = DefaultUserInterfaceStyle;

    /// <summary>
    /// Android only
    /// </summary>
    public Color BackgroundColor { get; set; } = DefaultBackgroundColor;

    /// <summary>
    /// Android only
    /// </summary>
    /// <remarks>
    /// Works only if <see cref="BackgroundColor"/> was set
    /// </remarks>
    public float CornerRadius { get; set; } = DefaultCornerRadius;

    public Color MessageColor { get; set; } = DefaultMessageColor;
    public Color TitleColor { get; set; } = DefaultTitleColor;
    public Color DestructiveButtonTextColor { get; set; } = DefaultDestructiveButtonTextColor;
    public Color NegativeButtonTextColor { get; set; } = DefaultNegativeButtonTextColor;
    public Color ActionSheetOptionTextColor { get; set; } = DefaultActionSheetOptionTextColor;
    public float TitleFontSize { get; set; } = DefaultTitleFontSize;
    public float MessageFontSize { get; set; } = DefaultMessageFontSize;

    /// <summary>
    /// Android only
    /// </summary>
    public float DestructiveButtonFontSize { get; set; } = DefaultDestructiveButtonFontSize;

    /// <summary>
    /// Android only
    /// </summary>
    public float NegativeButtonFontSize { get; set; } = DefaultNegativeButtonFontSize;

    /// <summary>
    /// Android only
    /// </summary>
    public float ActionSheetOptionFontSize { get; set; } = DefaultActionSheetOptionFontSize;

    /// <summary>
    /// Android only
    /// </summary>
    public Color ActionSheetSeparatorColor { get; set; } = DefaultActionSheetSeparatorColor;

    public static string DefaultCancelText { get; set; } = "Cancel";
    public static string DefaultDestructiveText { get; set; } = "Remove";

    public string Title { get; set; }

    /// <summary>
    /// Not used on android action sheet if UseBottomSheet is False
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Android only
    /// </summary>
    public string Icon { get; set; }
    public ActionSheetOption Cancel { get; set; }
    public ActionSheetOption Destructive { get; set; }
    public IList<ActionSheetOption> Options { get; set; } = new List<ActionSheetOption>();

    /// <summary>
    /// Android only
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