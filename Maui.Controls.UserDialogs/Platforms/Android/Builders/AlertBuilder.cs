using Android.App;
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
    public virtual Dialog Build(Activity activity, AlertConfig config)
    {
        var builder = new AlertDialog.Builder(activity)
            .SetCancelable(false);

        builder.SetMessage(GetMessage(config));

        if (config.Title != null) builder.SetTitle(GetTitle(config));

        if (config.Icon != null) builder.SetIcon(GetIcon(config));

        builder.SetPositiveButton(GetPositiveButton(config), (o, e) => config.Action?.Invoke());

        var dialog = builder.Create();
        dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));

        return dialog;
    }

    public virtual Dialog Build(AppCompatActivity activity, AlertConfig config)
    {
        var builder = new AppCompatAlertDialog.Builder(activity)
            .SetCancelable(false);

        builder.SetMessage(GetMessage(config));

        if (config.Title != null) builder.SetTitle(GetTitle(config));

        if (config.Icon != null) builder.SetIcon(GetIcon(config));

        builder.SetPositiveButton(GetPositiveButton(config), (o, e) => config.Action?.Invoke());

        var dialog = builder.Create();
        dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));

        return dialog;
    }

    protected virtual Drawable GetDialogBackground(AlertConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(AlertConfig.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(AlertConfig.CornerRadius));

        var draw = new InsetDrawable(backgroundDrawable, DpToPixels(20), 0, DpToPixels(20), 0);

        return draw;
    }

    protected virtual SpannableString GetMessage(AlertConfig config)
    {
        var messageSpan = new SpannableString(config.Message);

        messageSpan.SetSpan(new ForegroundColorSpan(AlertConfig.MessageColor.ToPlatform()), 0, config.Message.Length, SpanTypes.ExclusiveExclusive);
        messageSpan.SetSpan(new AbsoluteSizeSpan((int)AlertConfig.MessageFontSize, true), 0, config.Message.Length, SpanTypes.ExclusiveExclusive);

        return messageSpan;
    }

    protected virtual SpannableString GetTitle(AlertConfig config)
    {
        var titleSpan = new SpannableString(config.Title);

        titleSpan.SetSpan(new ForegroundColorSpan(AlertConfig.TitleColor.ToPlatform()), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);
        titleSpan.SetSpan(new AbsoluteSizeSpan((int)AlertConfig.TitleFontSize, true), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);

        return titleSpan;
    }

    protected virtual Drawable GetIcon(AlertConfig config)
    {
        var imgId = MauiApplication.Current.GetDrawableId(config.Icon);
        var img = MauiApplication.Current.GetDrawable(imgId);

        return img;
    }

    protected virtual SpannableString GetPositiveButton(AlertConfig config)
    {
        var buttonSpan = new SpannableString(config.OkText);

        buttonSpan.SetSpan(new ForegroundColorSpan(AlertConfig.PositiveButtonTextColor.ToPlatform()), 0, config.OkText.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)AlertConfig.PositiveButtonFontSize, true), 0, config.OkText.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, config.OkText.Length, SpanTypes.ExclusiveExclusive);

        return buttonSpan;
    }
}