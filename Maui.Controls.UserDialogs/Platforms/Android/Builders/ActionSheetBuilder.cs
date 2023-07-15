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

public class ActionSheetBuilder
{
    public virtual Dialog Build(Activity activity, ActionSheetConfig config)
    {
        var builder = new AlertDialog.Builder(activity);

        if (config.Title != null) builder.SetTitle(GetTitle(config));

        if (config.Icon != null) builder.SetIcon(GetIcon(config));

        builder.SetAdapter(GetActionsAdapter(activity, config), (s, a) => config.Options[a.Which].Action?.Invoke());

        if (config.Destructive != null)
        {
            builder.SetNegativeButton(GetDestructiveButton(config), (o, a) => config.Destructive.Action?.Invoke());
        }

        if (config.Cancel != null)
        {
            builder.SetNeutralButton(GetCancelButton(config), (o, a) => config.Cancel.Action?.Invoke());
        }

        var dialog = builder.Create();
        dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));

        return dialog;
    }

    public virtual Dialog Build(AppCompatActivity activity, ActionSheetConfig config)
    {
        var builder = new AppCompatAlertDialog.Builder(activity);

        if (config.Title != null) builder.SetTitle(GetTitle(config));

        if (config.Icon != null) builder.SetIcon(GetIcon(config));

        builder.SetAdapter(GetActionsAdapter(activity, config), (s, a) => config.Options[a.Which].Action?.Invoke());

        if (config.Destructive != null)
        {
            builder.SetNegativeButton(GetDestructiveButton(config), (o, a) => config.Destructive.Action?.Invoke());
        }

        if (config.Cancel != null)
        {
            builder.SetNeutralButton(GetCancelButton(config), (o, a) => config.Cancel.Action?.Invoke());
        }

        var dialog = builder.Create();
        dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));

        return dialog;
    }

    protected virtual ActionSheetListAdapter GetActionsAdapter(Android.Content.Context context, ActionSheetConfig config)
    {
        return new ActionSheetListAdapter(context, Android.Resource.Layout.SelectDialogItem, Android.Resource.Id.Text1, config);
    }

    protected virtual Drawable GetDialogBackground(ActionSheetConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(ActionSheetConfig.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(ActionSheetConfig.CornerRadius));

        var draw = new InsetDrawable(backgroundDrawable, DpToPixels(20), 0, DpToPixels(20), 0);

        return draw;
    }

    protected virtual SpannableString GetMessage(ActionSheetConfig config)
    {
        var messageSpan = new SpannableString(config.Message);

        messageSpan.SetSpan(new ForegroundColorSpan(ActionSheetConfig.MessageColor.ToPlatform()), 0, config.Message.Length, SpanTypes.ExclusiveExclusive);
        messageSpan.SetSpan(new AbsoluteSizeSpan((int)ActionSheetConfig.MessageFontSize, true), 0, config.Message.Length, SpanTypes.ExclusiveExclusive);

        return messageSpan;
    }

    protected virtual SpannableString GetTitle(ActionSheetConfig config)
    {
        var titleSpan = new SpannableString(config.Title);

        titleSpan.SetSpan(new ForegroundColorSpan(ActionSheetConfig.TitleColor.ToPlatform()), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);
        titleSpan.SetSpan(new AbsoluteSizeSpan((int)ActionSheetConfig.TitleFontSize, true), 0, config.Title.Length, SpanTypes.ExclusiveExclusive);

        return titleSpan;
    }

    protected virtual Drawable GetIcon(ActionSheetConfig config)
    {
        var imgId = MauiApplication.Current.GetDrawableId(config.Icon);
        var img = MauiApplication.Current.GetDrawable(imgId);

        return img;
    }

    protected virtual SpannableString GetCancelButton(ActionSheetConfig config)
    {
        var buttonSpan = new SpannableString(config.Cancel.Text);

        buttonSpan.SetSpan(new ForegroundColorSpan(ActionSheetConfig.NegativeButtonTextColor.ToPlatform()), 0, config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)ActionSheetConfig.NegativeButtonFontSize, true), 0, config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);

        return buttonSpan;
    }

    protected virtual SpannableString GetDestructiveButton(ActionSheetConfig config)
    {
        var buttonSpan = new SpannableString(config.Destructive.Text);

        buttonSpan.SetSpan(new ForegroundColorSpan(ActionSheetConfig.DestructiveButtonTextColor.ToPlatform()), 0, config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)ActionSheetConfig.DestructiveButtonFontSize, true), 0, config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);

        return buttonSpan;
    }
}