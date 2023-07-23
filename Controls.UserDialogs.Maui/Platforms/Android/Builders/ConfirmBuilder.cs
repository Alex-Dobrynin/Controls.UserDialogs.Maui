using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Style;

using AndroidX.AppCompat.App;

using Microsoft.Maui.Platform;

using static Controls.UserDialogs.Maui.Extensions;

using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Controls.UserDialogs.Maui;

public class ConfirmBuilder
{
    public static Thickness DefaultScreenMargin { get; set; } = new Thickness(20, 50);

    public Thickness ScreenMargin { get; set; } = DefaultScreenMargin;

    private Typeface _typeface;

    public virtual Dialog Build(Activity activity, ConfirmConfig config)
    {
        var builder = new AlertDialog.Builder(activity)
            .SetCancelable(false);

        if (config.FontFamily is not null)
        {
            _typeface = Typeface.CreateFromAsset(activity.Assets, config.FontFamily);
        }

        builder.SetMessage(GetMessage(config));

        if (config.Title is not null) builder.SetTitle(GetTitle(activity, config));

        if (config.Icon is not null) builder.SetIcon(GetIcon(config));

        builder.SetPositiveButton(GetPositiveButton(config), (o, e) => config.Action?.Invoke(true));

        builder.SetNeutralButton(GetNegativeButton(config), (o, e) => config.Action?.Invoke(false));

        var dialog = builder.Create();

        if (config.BackgroundColor is not null)
        {
            dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));
        }

        return dialog;
    }

    public virtual Dialog Build(AppCompatActivity activity, ConfirmConfig config)
    {
        var builder = new AppCompatAlertDialog.Builder(activity)
            .SetCancelable(false);

        if (config.FontFamily is not null)
        {
            _typeface = Typeface.CreateFromAsset(activity.Assets, config.FontFamily);
        }

        builder.SetMessage(GetMessage(config));

        if (config.Title is not null) builder.SetTitle(GetTitle(activity, config));

        if (config.Icon is not null) builder.SetIcon(GetIcon(config));

        builder.SetPositiveButton(GetPositiveButton(config), (o, e) => config.Action?.Invoke(true));

        builder.SetNeutralButton(GetNegativeButton(config), (o, e) => config.Action?.Invoke(false));

        var dialog = builder.Create();

        if (config.BackgroundColor is not null)
        {
            dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));
        }

        return dialog;
    }

    protected virtual Drawable GetDialogBackground(ConfirmConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(config.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(config.CornerRadius));

        var draw = new InsetDrawable(backgroundDrawable, DpToPixels(ScreenMargin.Left), DpToPixels(ScreenMargin.Top), DpToPixels(ScreenMargin.Right), DpToPixels(ScreenMargin.Bottom));

        return draw;
    }

    protected virtual SpannableString GetMessage(ConfirmConfig config)
    {
        var messageSpan = new SpannableString(config.Message);

        if (config.MessageColor is not null)
        {
            messageSpan.SetSpan(new ForegroundColorSpan(config.MessageColor.ToPlatform()), 0, config.Message.Length, SpanTypes.ExclusiveExclusive);
        }
        messageSpan.SetSpan(new AbsoluteSizeSpan((int)config.MessageFontSize, true), 0, config.Message.Length, SpanTypes.ExclusiveExclusive);

        if (config.FontFamily is not null)
        {
            messageSpan.SetSpan(new CustomTypeFaceSpan(_typeface), 0, config.Message.Length, SpanTypes.ExclusiveExclusive);
        }

        return messageSpan;
    }

    protected virtual SpannableString GetTitle(Activity activity, ConfirmConfig config)
    {
        var titleSpan = new SpannableString(config.Title);

        if (config.TitleColor is not null)
        {
            titleSpan.SetSpan(new ForegroundColorSpan(config.TitleColor.ToPlatform()), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);
        }
        titleSpan.SetSpan(new AbsoluteSizeSpan((int)config.TitleFontSize, true), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);

        if (config.FontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(activity.Assets, config.FontFamily);
            titleSpan.SetSpan(new CustomTypeFaceSpan(typeface), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);
            titleSpan.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);
        }

        return titleSpan;
    }

    protected virtual Drawable GetIcon(ConfirmConfig config)
    {
        var imgId = MauiApplication.Current.GetDrawableId(config.Icon);
        var img = MauiApplication.Current.GetDrawable(imgId);

        return img;
    }

    protected virtual SpannableString GetPositiveButton(ConfirmConfig config)
    {
        var buttonSpan = new SpannableString(config.OkText);

        if (config.PositiveButtonTextColor is not null)
        {
            buttonSpan.SetSpan(new ForegroundColorSpan(config.PositiveButtonTextColor.ToPlatform()), 0, config.OkText.Length, SpanTypes.ExclusiveExclusive);
        }
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)config.PositiveButtonFontSize, true), 0, config.OkText.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, config.OkText.Length, SpanTypes.ExclusiveExclusive);
        if (config.FontFamily is not null)
        {
            buttonSpan.SetSpan(new CustomTypeFaceSpan(_typeface), 0, config.OkText.Length, SpanTypes.ExclusiveExclusive);
        }

        return buttonSpan;
    }

    protected virtual SpannableString GetNegativeButton(ConfirmConfig config)
    {
        var buttonSpan = new SpannableString(config.CancelText);

        if (config.NegativeButtonTextColor is not null)
        {
            buttonSpan.SetSpan(new ForegroundColorSpan(config.NegativeButtonTextColor.ToPlatform()), 0, config.CancelText.Length, SpanTypes.ExclusiveExclusive);
        }
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)config.NegativeButtonFontSize, true), 0, config.CancelText.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, config.CancelText.Length, SpanTypes.ExclusiveExclusive);
        if (config.FontFamily is not null)
        {
            buttonSpan.SetSpan(new CustomTypeFaceSpan(_typeface), 0, config.CancelText.Length, SpanTypes.ExclusiveExclusive);
        }

        return buttonSpan;
    }
}