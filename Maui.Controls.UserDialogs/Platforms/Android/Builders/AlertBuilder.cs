using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Style;

using AndroidX.AppCompat.App;

using Microsoft.Maui.Platform;

using static Maui.Controls.UserDialogs.Extensions;

using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Maui.Controls.UserDialogs;

public class AlertBuilder
{
    private Typeface _typeface;

    public virtual Dialog Build(Activity activity, AlertConfig config)
    {
        var builder = new AlertDialog.Builder(activity)
            .SetCancelable(false);

        if (config.FontFamily is not null)
        {
            _typeface = Typeface.CreateFromAsset(activity.Assets, config.FontFamily);
        }

        builder.SetMessage(GetMessage(config));

        if (config.Title is not null) builder.SetTitle(GetTitle(config));

        if (config.Icon is not null) builder.SetIcon(GetIcon(config));

        builder.SetPositiveButton(GetPositiveButton(activity, config), (o, e) => config.Action?.Invoke());

        var dialog = builder.Create();

        if (config.BackgroundColor is not null)
        {
            dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));
        }

        return dialog;
    }

    public virtual Dialog Build(AppCompatActivity activity, AlertConfig config)
    {
        var builder = new AppCompatAlertDialog.Builder(activity)
            .SetCancelable(false);

        if (config.FontFamily is not null)
        {
            _typeface = Typeface.CreateFromAsset(activity.Assets, config.FontFamily);
        }

        builder.SetMessage(GetMessage(config));

        if (config.Title is not null) builder.SetTitle(GetTitle(config));

        if (config.Icon is not null) builder.SetIcon(GetIcon(config));

        builder.SetPositiveButton(GetPositiveButton(activity, config), (o, e) => config.Action?.Invoke());

        var dialog = builder.Create();

        if (config.BackgroundColor is not null)
        {
            dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));
        }

        return dialog;
    }

    protected virtual Drawable GetDialogBackground(AlertConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(config.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(config.CornerRadius));

        var draw = new InsetDrawable(backgroundDrawable, DpToPixels(20), 0, DpToPixels(20), 0);

        return draw;
    }

    protected virtual SpannableString GetMessage(AlertConfig config)
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

    protected virtual SpannableString GetTitle(AlertConfig config)
    {
        var titleSpan = new SpannableString(config.Title);

        if (config.TitleColor is not null)
        {
            titleSpan.SetSpan(new ForegroundColorSpan(config.TitleColor.ToPlatform()), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);
        }

        titleSpan.SetSpan(new AbsoluteSizeSpan((int)config.TitleFontSize, true), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);

        if (config.FontFamily is not null)
        {
            titleSpan.SetSpan(new CustomTypeFaceSpan(_typeface), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);
        }

        return titleSpan;
    }

    protected virtual Drawable GetIcon(AlertConfig config)
    {
        var imgId = MauiApplication.Current.GetDrawableId(config.Icon);
        var img = MauiApplication.Current.GetDrawable(imgId);

        return img;
    }

    protected virtual SpannableString GetPositiveButton(Activity activity, AlertConfig config)
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
}