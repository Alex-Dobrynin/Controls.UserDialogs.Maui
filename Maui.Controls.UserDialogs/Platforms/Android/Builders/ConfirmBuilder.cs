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

public class ConfirmBuilder
{
    public virtual Dialog Build(Activity activity, ConfirmConfig config)
    {
        var builder = new AlertDialog.Builder(activity)
            .SetCancelable(false);

        builder.SetMessage(GetMessage(config));

        if (config.Title != null) builder.SetTitle(GetTitle(config));

        if (config.Icon != null) builder.SetIcon(GetIcon(config));

        builder.SetPositiveButton(GetPositiveButton(config), (o, e) => config.Action?.Invoke(true));

        builder.SetNeutralButton(GetNegativeButton(config), (o, e) => config.Action?.Invoke(false));

        var dialog = builder.Create();
        dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));

        return dialog;
    }

    public virtual Dialog Build(AppCompatActivity activity, ConfirmConfig config)
    {
        var builder = new AppCompatAlertDialog.Builder(activity)
            .SetCancelable(false);

        builder.SetMessage(GetMessage(config));

        if (config.Title != null) builder.SetTitle(GetTitle(config));

        if (config.Icon != null) builder.SetIcon(GetIcon(config));

        builder.SetPositiveButton(GetPositiveButton(config), (o, e) => config.Action?.Invoke(true));

        builder.SetNeutralButton(GetNegativeButton(config), (o, e) => config.Action?.Invoke(false));

        var dialog = builder.Create();
        dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));

        return dialog;
    }

    protected virtual Drawable GetDialogBackground(ConfirmConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(ConfirmConfig.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(ConfirmConfig.CornerRadius));

        var draw = new InsetDrawable(backgroundDrawable, DpToPixels(20), 0, DpToPixels(20), 0);

        return draw;
    }

    protected virtual SpannableString GetMessage(ConfirmConfig config)
    {
        var messageSpan = new SpannableString(config.Message);

        messageSpan.SetSpan(new ForegroundColorSpan(ConfirmConfig.MessageColor.ToPlatform()), 0, config.Message.Length, SpanTypes.ExclusiveExclusive);
        messageSpan.SetSpan(new AbsoluteSizeSpan((int)ConfirmConfig.MessageFontSize, true), 0, config.Message.Length, SpanTypes.ExclusiveExclusive);

        return messageSpan;
    }

    protected virtual SpannableString GetTitle(ConfirmConfig config)
    {
        var titleSpan = new SpannableString(config.Title);

        titleSpan.SetSpan(new ForegroundColorSpan(ConfirmConfig.TitleColor.ToPlatform()), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);
        titleSpan.SetSpan(new AbsoluteSizeSpan((int)ConfirmConfig.TitleFontSize, true), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);

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

        buttonSpan.SetSpan(new ForegroundColorSpan(ConfirmConfig.PositiveButtonTextColor.ToPlatform()), 0, config.OkText.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)ConfirmConfig.PositiveButtonFontSize, true), 0, config.OkText.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, config.OkText.Length, SpanTypes.ExclusiveExclusive);

        return buttonSpan;
    }

    protected virtual SpannableString GetNegativeButton(ConfirmConfig config)
    {
        var buttonSpan = new SpannableString(config.CancelText);

        buttonSpan.SetSpan(new ForegroundColorSpan(ConfirmConfig.NegativeButtonTextColor.ToPlatform()), 0, config.CancelText.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)ConfirmConfig.NegativeButtonFontSize, true), 0, config.CancelText.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, config.CancelText.Length, SpanTypes.ExclusiveExclusive);

        return buttonSpan;
    }
}