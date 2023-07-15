namespace Maui.Controls.UserDialogs;

public class ConfirmConfig
{
    public static float CornerRadius { get; set; } = 15;
    public static Color BackgroundColor { get; set; } = Colors.White;
    public static double TitleFontSize { get; set; } = 20;
    public static Color TitleColor { get; set; } = Colors.Black;
    public static double MessageFontSize { get; set; } = 16;
    public static Color MessageColor { get; set; } = Colors.Black;
    public static Color PositiveButtonTextColor { get; set; } = Colors.Black;
    public static double PositiveButtonFontSize { get; set; } = 18;
    public static Color NegativeButtonTextColor { get; set; } = Colors.Black;
    public static double NegativeButtonFontSize { get; set; } = 18;

    public static bool DefaultUseYesNo { get; set; }
    public static string DefaultYes { get; set; } = "Yes";
    public static string DefaultNo { get; set; } = "No";
    public static string DefaultOkText { get; set; } = "Ok";
    public static string DefaultCancelText { get; set; } = "Cancel";

    public string OkText { get; set; } = DefaultUseYesNo ? DefaultYes : DefaultOkText;
    public string Title { get; set; }
    public string Message { get; set; }
    public string Icon { get; set; }
    public Action<bool> Action { get; set; }

    public string CancelText { get; set; } = DefaultUseYesNo ? DefaultNo : DefaultCancelText;

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