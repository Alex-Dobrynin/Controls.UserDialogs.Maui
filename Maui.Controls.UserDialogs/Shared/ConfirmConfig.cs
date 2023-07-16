namespace Maui.Controls.UserDialogs;

public class ConfirmConfig
{
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
    public static double DefaultTitleFontSize { get; set; } = 20;
    public static double DefaultMessageFontSize { get; set; } = 16;
    public static Color DefaultPositiveButtonTextColor { get; set; }
    public static double DefaultPositiveButtonFontSize { get; set; } = 18;
    public static Color DefaultNegativeButtonTextColor { get; set; }
    public static double DefaultNegativeButtonFontSize { get; set; } = 18;

    public string FontFamily { get; set; } = DefaultFontFamily;
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
    public Color NegativeButtonTextColor { get; set; } = DefaultNegativeButtonTextColor;
    public double TitleFontSize { get; set; } = DefaultTitleFontSize;
    public double MessageFontSize { get; set; } = DefaultMessageFontSize;
    public double PositiveButtonFontSize { get; set; } = DefaultPositiveButtonFontSize;
    public double NegativeButtonFontSize { get; set; } = DefaultNegativeButtonFontSize;

    public static bool DefaultUseYesNo { get; set; }
    public static string DefaultYes { get; set; } = "Yes";
    public static string DefaultNo { get; set; } = "No";
    public static string DefaultOkText { get; set; } = "Ok";
    public static string DefaultCancelText { get; set; } = "Cancel";

    public string OkText { get; set; } = DefaultUseYesNo ? DefaultYes : DefaultOkText;
    public string CancelText { get; set; } = DefaultUseYesNo ? DefaultNo : DefaultCancelText;
    public string Title { get; set; }
    public string Message { get; set; }
    public string Icon { get; set; }
    public Action<bool> Action { get; set; }

    public ConfirmConfig UseYesNo()
    {
        this.OkText = DefaultYes;
        this.CancelText = DefaultNo;
        return this;
    }

    public ConfirmConfig SetOkText(string text)
    {
        this.OkText = text;
        return this;
    }

    public ConfirmConfig SetTitle(string title)
    {
        this.Title = title;
        return this;
    }

    public ConfirmConfig SetMessage(string message)
    {
        this.Message = message;
        return this;
    }

    public ConfirmConfig SetIcon(string icon)
    {
        this.Icon = icon;
        return this;
    }

    public ConfirmConfig SetAction(Action<bool> action)
    {
        this.Action = action;
        return this;
    }

    public ConfirmConfig SetCancelText(string text)
    {
        this.CancelText = text;
        return this;
    }
}