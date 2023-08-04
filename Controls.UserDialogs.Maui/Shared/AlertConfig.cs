namespace Controls.UserDialogs.Maui;

public class AlertConfig
{
    public static string DefaultTitleFontFamily { get; set; }
    public static string DefaultFontFamily { get; set; }
    public static string DefaultPositiveButtonFontFamily { get; set; }

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
    public static float DefaultMessageFontSize { get; set; } = 16;
    public static Color DefaultPositiveButtonTextColor { get; set; }

    /// <summary>
    /// Android only
    /// </summary>
    public static float DefaultPositiveButtonFontSize { get; set; } = 18;

    public string TitleFontFamily { get; set; } = DefaultTitleFontFamily;
    public string FontFamily { get; set; } = DefaultFontFamily;
    public string PositiveButtonFontFamily { get; set; } = DefaultPositiveButtonFontFamily;
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
    public Color PositiveButtonTextColor { get; set; } = DefaultPositiveButtonTextColor;
    public float TitleFontSize { get; set; } = DefaultTitleFontSize;
    public float MessageFontSize { get; set; } = DefaultMessageFontSize;

    /// <summary>
    /// Android only
    /// </summary>
    public float PositiveButtonFontSize { get; set; } = DefaultPositiveButtonFontSize;

    public static string DefaultOkText { get; set; } = "Ok";

    public string OkText { get; set; } = DefaultOkText;
    public string Title { get; set; }
    public string Message { get; set; }
    /// <summary>
    /// Android only
    /// </summary>
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