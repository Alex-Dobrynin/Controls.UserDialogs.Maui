namespace Maui.Controls.UserDialogs;

public class HudDialogConfig
{
    public static float CornerRadius { get; set; } = 15;
    public static Color BackgroundColor { get; set; } = Colors.Black.WithAlpha(0.5f);
    public static double TitleFontSize { get; set; } = 16;
    public static Color TitleColor { get; set; } = Colors.White;
    public static double MessageFontSize { get; set; } = 14;
    public static Color MessageColor { get; set; } = Colors.White;
    public static Color LoaderColor { get; set; } = Colors.White;
    public static Color ProgressColor { get; set; } = Colors.White;
    public static Color NegativeButtonTextColor { get; set; } = Colors.White;
    public static double NegativeButtonFontSize { get; set; } = 18;

    public static string DefaultCancelText { get; set; } = "Cancel";
    public static string DefaultTitle { get; set; }
    public static MaskType DefaultMaskType { get; set; } = MaskType.Black;

    public string CancelText { get; set; } = DefaultCancelText;
    public string Title { get; set; } = DefaultTitle;
    public string Message { get; set; }
    public string Image { get; set; }
    public bool AutoShow { get; set; } = true;
    public int PercentComplete { get; set; } = -1;
    public MaskType MaskType { get; set; } = DefaultMaskType;
    public Action OnCancel { get; set; }

    public HudDialogConfig()
    {

    }

    public HudDialogConfig SetCancel(string cancelText = null, Action onCancel = null)
    {
        if (cancelText is not null)
            this.CancelText = cancelText;

        this.OnCancel = onCancel;
        return this;
    }

    public HudDialogConfig SetTitle(string title)
    {
        this.Title = title;
        return this;
    }

    public HudDialogConfig SetMessage(string message)
    {
        this.Message = message;
        return this;
    }

    public HudDialogConfig SetMaskType(MaskType maskType)
    {
        this.MaskType = maskType;
        return this;
    }

    public HudDialogConfig SetAutoShow(bool autoShow)
    {
        this.AutoShow = autoShow;
        return this;
    }
}