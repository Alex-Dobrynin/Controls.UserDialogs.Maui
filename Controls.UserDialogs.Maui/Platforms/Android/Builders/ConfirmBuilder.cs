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

public class ConfirmBuilder
{
    public static Thickness DefaultScreenMargin { get; set; } = new Thickness(20, 50);

    public Thickness ScreenMargin { get; set; } = DefaultScreenMargin;

    protected Activity Activity { get; }
    protected ConfirmConfig Config { get; }

    public ConfirmBuilder(Activity activity, ConfirmConfig config)
    {
        Activity = activity;
        Config = config;
    }

    public virtual Dialog Build()
    {
        var builder = new AlertDialog.Builder(Activity)
            .SetCancelable(false);

        builder.SetMessage(GetMessage());

        if (Config.Title is not null) builder.SetTitle(GetTitle());

        if (Config.Icon is not null) builder.SetIcon(GetIcon());

        builder.SetPositiveButton(GetPositiveButton(), (o, e) => Config.Action?.Invoke(true));

        builder.SetNeutralButton(GetNegativeButton(), (o, e) => Config.Action?.Invoke(false));

        var dialog = builder.Create();

        if (Config.BackgroundColor is not null)
        {
            dialog.Window.SetBackgroundDrawable(GetDialogBackground());
        }

        return dialog;
    }

    public virtual Dialog BuildAppCompat()
    {
        var builder = new AppCompatAlertDialog.Builder(Activity)
            .SetCancelable(false);

        builder.SetMessage(GetMessage());

        if (Config.Title is not null) builder.SetTitle(GetTitle());

        if (Config.Icon is not null) builder.SetIcon(GetIcon());

        builder.SetPositiveButton(GetPositiveButton(), (o, e) => Config.Action?.Invoke(true));

        builder.SetNeutralButton(GetNegativeButton(), (o, e) => Config.Action?.Invoke(false));

        var dialog = builder.Create();

        if (Config.BackgroundColor is not null)
        {
            dialog.Window.SetBackgroundDrawable(GetDialogBackground());
        }

        return dialog;
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

    protected virtual SpannableString GetPositiveButton()
    {
        var buttonSpan = new SpannableString(Config.OkText);

        if (Config.PositiveButtonTextColor is not null)
        {
            buttonSpan.SetSpan(new ForegroundColorSpan(Config.PositiveButtonTextColor.ToPlatform()), 0, Config.OkText.Length, SpanTypes.ExclusiveExclusive);
        }
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)Config.PositiveButtonFontSize, true), 0, Config.OkText.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, Config.OkText.Length, SpanTypes.ExclusiveExclusive);

        if (Config.PositiveButtonFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity.Assets, Config.PositiveButtonFontFamily);
            buttonSpan.SetSpan(new CustomTypeFaceSpan(typeface), 0, Config.OkText.Length, SpanTypes.ExclusiveExclusive);
        }

        return buttonSpan;
    }

    protected virtual SpannableString GetNegativeButton()
    {
        var buttonSpan = new SpannableString(Config.CancelText);

        if (Config.NegativeButtonTextColor is not null)
        {
            buttonSpan.SetSpan(new ForegroundColorSpan(Config.NegativeButtonTextColor.ToPlatform()), 0, Config.CancelText.Length, SpanTypes.ExclusiveExclusive);
        }
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)Config.NegativeButtonFontSize, true), 0, Config.CancelText.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, Config.CancelText.Length, SpanTypes.ExclusiveExclusive);

        if (Config.NegativeButtonFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity.Assets, Config.NegativeButtonFontFamily);
            buttonSpan.SetSpan(new CustomTypeFaceSpan(typeface), 0, Config.CancelText.Length, SpanTypes.ExclusiveExclusive);
        }

        return buttonSpan;
    }
}