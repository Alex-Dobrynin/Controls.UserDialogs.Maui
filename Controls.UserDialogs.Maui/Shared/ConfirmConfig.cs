namespace Controls.UserDialogs.Maui;

public class ConfirmConfig
{
    public static string? DefaultTitleFontFamily { get; set; }
    public static string? DefaultMessageFontFamily { get; set; }
    public static string? DefaultPositiveButtonFontFamily { get; set; }
    public static string? DefaultNegativeButtonFontFamily { get; set; }

    /// <summary>
    /// iOS only
    /// </summary>
    public static UserInterfaceStyle? DefaultUserInterfaceStyle { get; set; }

    /// <summary>
    /// Android only
    /// </summary>
    public static Color? DefaultBackgroundColor { get; set; }

    /// <summary>
    /// Android only
    /// </summary>
    public static float DefaultCornerRadius { get; set; } = 15;

    public static Color? DefaultMessageColor { get; set; }
    public static Color? DefaultTitleColor { get; set; }
    public static float DefaultTitleFontSize { get; set; } = 20;
    public static float DefaultMessageFontSize { get; set; } = 16;

    /// <summary>
    /// Android only
    /// </summary>
    public static Color? DefaultPositiveButtonTextColor { get; set; }
    public static float DefaultPositiveButtonFontSize { get; set; } = 18;

    /// <summary>
    /// Android only
    /// </summary>
    public static Color? DefaultNegativeButtonTextColor { get; set; }
    public static float DefaultNegativeButtonFontSize { get; set; } = 18;

    public string? TitleFontFamily { get; set; } = DefaultTitleFontFamily;
    public string? MessageFontFamily { get; set; } = DefaultMessageFontFamily;
    public string? PositiveButtonFontFamily { get; set; } = DefaultPositiveButtonFontFamily;
    public string? NegativeButtonFontFamily { get; set; } = DefaultNegativeButtonFontFamily;
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
    public Color? BackgroundColor { get; set; } = DefaultBackgroundColor;

    /// <summary>
    /// Android only
    /// </summary>
    /// <remarks>
    /// Works only if <see cref="BackgroundColor"/> was set
    /// </remarks>
    public float CornerRadius { get; set; } = DefaultCornerRadius;

    public Color? MessageColor { get; set; } = DefaultMessageColor;
    public Color? TitleColor { get; set; } = DefaultTitleColor;

    /// <summary>
    /// Android only
    /// </summary>
    public Color? PositiveButtonTextColor { get; set; } = DefaultPositiveButtonTextColor;

    /// <summary>
    /// Android only
    /// </summary>
    public Color? NegativeButtonTextColor { get; set; } = DefaultNegativeButtonTextColor;
    public float TitleFontSize { get; set; } = DefaultTitleFontSize;
    public float MessageFontSize { get; set; } = DefaultMessageFontSize;
    public float PositiveButtonFontSize { get; set; } = DefaultPositiveButtonFontSize;
    public float NegativeButtonFontSize { get; set; } = DefaultNegativeButtonFontSize;

    public static bool DefaultUseYesNo { get; set; }
    public static string DefaultYes { get; set; } = "Yes";
    public static string DefaultNo { get; set; } = "No";
    public static string DefaultOkText { get; set; } = "Ok";
    public static string DefaultCancelText { get; set; } = "Cancel";

    public string OkText { get; set; } = DefaultUseYesNo ? DefaultYes : DefaultOkText;
    public string CancelText { get; set; } = DefaultUseYesNo ? DefaultNo : DefaultCancelText;
    public string? Title { get; set; }
    public string Message { get; set; }

    /// <summary>
    /// Android only
    /// </summary>
    public string? Icon { get; set; }
    public Action<bool>? Action { get; set; }

    public ConfirmConfig UseYesNo()
    {
        OkText = DefaultYes;
        CancelText = DefaultNo;
        return this;
    }

    public ConfirmConfig SetOkText(string text)
    {
        OkText = text;
        return this;
    }

    public ConfirmConfig SetTitle(string title)
    {
        Title = title;
        return this;
    }

    public ConfirmConfig SetMessage(string message)
    {
        Message = message;
        return this;
    }

    public ConfirmConfig SetIcon(string icon)
    {
        Icon = icon;
        return this;
    }

    public ConfirmConfig SetAction(Action<bool> action)
    {
        Action = action;
        return this;
    }

    public ConfirmConfig SetCancelText(string text)
    {
        CancelText = text;
        return this;
    }
}