using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Style;

using Microsoft.Maui.Platform;

using static Controls.UserDialogs.Maui.Extensions;

using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Controls.UserDialogs.Maui;

public class ActionSheetBuilder
{
    public static Thickness DefaultScreenMargin { get; set; } = new Thickness(20, 50);
    public static Thickness DefaultPadding { get; set; } = new Thickness(20, 18);
    public static double DefaultIconPadding { get; set; } = 10;
    public static double DefaultOptionIconPadding { get; set; } = 10;
    public static double DefaultOptionIconSize { get; set; } = 24;

    public Thickness ScreenMargin { get; set; } = DefaultScreenMargin;
    public Thickness Padding { get; set; } = DefaultPadding;
    public double IconPadding { get; set; } = DefaultIconPadding;
    public double OptionIconPadding { get; set; } = DefaultOptionIconPadding;
    public double OptionIconSize { get; set; } = DefaultOptionIconSize;

    protected Activity Activity { get; }
    protected ActionSheetConfig Config { get; }

    public ActionSheetBuilder(Activity activity, ActionSheetConfig config)
    {
        Activity = activity;
        Config = config;
    }

    public virtual Dialog Build()
    {
        var builder = new AlertDialog.Builder(Activity);

        if (Config.Title is not null) builder.SetTitle(GetTitle());

        if (Config.Icon is not null) builder.SetIcon(GetIcon());

        builder.SetAdapter(GetActionsAdapter(), (s, a) => Config.Options[a.Which].Action?.Invoke());

        if (Config.Destructive is not null)
        {
            builder.SetNegativeButton(GetDestructiveButton(), (o, a) => Config.Destructive.Action?.Invoke());
        }

        if (Config.Cancel is not null)
        {
            builder.SetNeutralButton(GetCancelButton(), (o, a) => Config.Cancel.Action?.Invoke());
        }

        var dialog = builder.Create();

        if (Config.BackgroundColor is not null)
        {
            dialog.Window.SetBackgroundDrawable(GetDialogBackground());
        }

        return dialog;
    }

    public virtual Dialog BuildAppCompat()
    {
        var builder = new AppCompatAlertDialog.Builder(Activity);

        if (Config.Title is not null) builder.SetTitle(GetTitle());

        if (Config.Icon is not null) builder.SetIcon(GetIcon());

        builder.SetAdapter(GetActionsAdapter(), (s, a) => Config.Options[a.Which].Action?.Invoke());

        if (Config.Destructive is not null)
        {
            builder.SetNegativeButton(GetDestructiveButton(), (o, a) => Config.Destructive.Action?.Invoke());
        }

        if (Config.Cancel is not null)
        {
            builder.SetNeutralButton(GetCancelButton(), (o, a) => Config.Cancel.Action?.Invoke());
        }

        var dialog = builder.Create();

        if (Config.BackgroundColor is not null)
        {
            dialog.Window.SetBackgroundDrawable(GetDialogBackground());
        }

        return dialog;
    }

    protected virtual ActionSheetListAdapter GetActionsAdapter()
    {
        return new ActionSheetListAdapter(Activity, Android.Resource.Layout.SelectDialogItem, Android.Resource.Id.Text1, this, Config);
    }

    protected virtual Drawable GetDialogBackground()
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(Config.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(Config.CornerRadius));

        var draw = new InsetDrawable(backgroundDrawable, DpToPixels(ScreenMargin.Left), DpToPixels(ScreenMargin.Top), DpToPixels(ScreenMargin.Right), DpToPixels(ScreenMargin.Bottom));

        return draw;
    }

    protected virtual SpannableString GetMessage()
    {
        var messageSpan = new SpannableString(Config.Message);

        if (Config.MessageColor is not null)
        {
            messageSpan.SetSpan(new ForegroundColorSpan(Config.MessageColor.ToPlatform()), 0, Config.Message.Length, SpanTypes.ExclusiveExclusive);
        }
        messageSpan.SetSpan(new AbsoluteSizeSpan((int)Config.MessageFontSize, true), 0, Config.Message.Length, SpanTypes.ExclusiveExclusive);

        if (Config.MessageFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity.Assets, Config.MessageFontFamily);
            messageSpan.SetSpan(new CustomTypeFaceSpan(typeface), 0, Config.Message.Length, SpanTypes.ExclusiveExclusive);
        }

        return messageSpan;
    }

    protected virtual SpannableString GetTitle()
    {
        var titleSpan = new SpannableString(Config.Title);

        if (Config.TitleColor is not null)
        {
            titleSpan.SetSpan(new ForegroundColorSpan(Config.TitleColor.ToPlatform()), 0, Config.Title.Length, SpanTypes.ExclusiveExclusive);
        }
        titleSpan.SetSpan(new AbsoluteSizeSpan((int)Config.TitleFontSize, true), 0, Config.Title.Length, SpanTypes.ExclusiveExclusive);

        if (Config.TitleFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity.Assets, Config.TitleFontFamily);
            titleSpan.SetSpan(new CustomTypeFaceSpan(typeface), 0, Config.Title.Length, SpanTypes.ExclusiveExclusive);
            titleSpan.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, Config.Title.Length, SpanTypes.ExclusiveExclusive);
        }

        return titleSpan;
    }

    protected virtual Drawable GetIcon()
    {
        var imgId = MauiApplication.Current.GetDrawableId(Config.Icon);
        var img = MauiApplication.Current.GetDrawable(imgId);

        return img;
    }

    protected virtual SpannableString GetCancelButton()
    {
        var buttonSpan = new SpannableString(Config.Cancel.Text);

        if (Config.NegativeButtonTextColor is not null)
        {
            buttonSpan.SetSpan(new ForegroundColorSpan(Config.NegativeButtonTextColor.ToPlatform()), 0, Config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);
        }
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)Config.NegativeButtonFontSize, true), 0, Config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, Config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);

        if (Config.NegativeButtonFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity.Assets, Config.NegativeButtonFontFamily);
            buttonSpan.SetSpan(new CustomTypeFaceSpan(typeface), 0, Config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);
        }

        return buttonSpan;
    }

    protected virtual SpannableString GetDestructiveButton()
    {
        var buttonSpan = new SpannableString(Config.Destructive.Text);

        if (Config.DestructiveButtonTextColor is not null)
        {
            buttonSpan.SetSpan(new ForegroundColorSpan(Config.DestructiveButtonTextColor.ToPlatform()), 0, Config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);
        }
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)Config.DestructiveButtonFontSize, true), 0, Config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, Config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);

        if (Config.DestructiveButtonFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity.Assets, Config.DestructiveButtonFontFamily);
            buttonSpan.SetSpan(new CustomTypeFaceSpan(typeface), 0, Config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);
        }

        return buttonSpan;
    }
}