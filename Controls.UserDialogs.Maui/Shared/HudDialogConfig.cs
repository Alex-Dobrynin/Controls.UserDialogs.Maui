﻿namespace Controls.UserDialogs.Maui;

public class HudDialogConfig
{
    public static string? DefaultMessageFontFamily { get; set; }
    public static string? DefaultNegativeButtonFontFamily { get; set; }
    public static float DefaultCornerRadius { get; set; } = 15;
    public static Color DefaultBackgroundColor { get; set; } = Colors.Black.WithAlpha(0.5f);
    public static double DefaultMessageFontSize { get; set; } = 14;
    public static Color DefaultMessageColor { get; set; } = Colors.White;
    public static Color DefaultLoaderColor { get; set; } = Colors.White;
    public static Color DefaultProgressColor { get; set; } = Colors.White;
    public static Color DefaultNegativeButtonTextColor { get; set; } = Colors.White;
    public static double DefaultNegativeButtonFontSize { get; set; } = 18;

    public static string DefaultCancelText { get; set; } = "Cancel";
    public static MaskType DefaultMaskType { get; set; } = MaskType.Black;

    public string? CancelText { get; set; } = DefaultCancelText;
    public string? Message { get; set; }
    public string? Image { get; set; }
    public bool AutoShow { get; set; } = true;
    public int PercentComplete { get; set; } = -1;
    public MaskType MaskType { get; set; } = DefaultMaskType;
    /// <summary>
    /// Doesn't work within ios image hud
    /// </summary>
    public Action? Cancel { get; set; }
    public string? MessageFontFamily { get; set; } = DefaultMessageFontFamily;
    public string? NegativeButtonFontFamily { get; set; } = DefaultNegativeButtonFontFamily;
    public float CornerRadius { get; set; } = DefaultCornerRadius;
    public Color BackgroundColor { get; set; } = DefaultBackgroundColor;
    public double MessageFontSize { get; set; } = DefaultMessageFontSize;
    public Color MessageColor { get; set; } = DefaultMessageColor;
    public Color LoaderColor { get; set; } = DefaultLoaderColor;
    public Color ProgressColor { get; set; } = DefaultProgressColor;
    public Color NegativeButtonTextColor { get; set; } = DefaultNegativeButtonTextColor;
    public double NegativeButtonFontSize { get; set; } = DefaultNegativeButtonFontSize;

    public HudDialogConfig SetCancel(string? cancelText = null, Action? onCancel = null)
    {
        if (cancelText is not null)
            CancelText = cancelText;

        Cancel = onCancel;
        return this;
    }

    public HudDialogConfig SetMessage(string message)
    {
        Message = message;
        return this;
    }

    public HudDialogConfig SetMaskType(MaskType maskType)
    {
        MaskType = maskType;
        return this;
    }

    public HudDialogConfig SetAutoShow(bool autoShow)
    {
        AutoShow = autoShow;
        return this;
    }
}