using System;

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

    private Typeface _typeface;

    public virtual Dialog Build(Activity activity, ActionSheetConfig config)
    {
        var builder = new AlertDialog.Builder(activity);

        if (config.FontFamily is not null)
        {
            _typeface = Typeface.CreateFromAsset(activity.Assets, config.FontFamily);
        }

        if (config.Title is not null) builder.SetTitle(GetTitle(config));

        if (config.Icon is not null) builder.SetIcon(GetIcon(config));

        builder.SetAdapter(GetActionsAdapter(activity, config), (s, a) => config.Options[a.Which].Action?.Invoke());

        if (config.Destructive is not null)
        {
            builder.SetNegativeButton(GetDestructiveButton(config), (o, a) => config.Destructive.Action?.Invoke());
        }

        if (config.Cancel is not null)
        {
            builder.SetNeutralButton(GetCancelButton(config), (o, a) => config.Cancel.Action?.Invoke());
        }

        var dialog = builder.Create();

        if (config.BackgroundColor is not null)
        {
            dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));
        }

        return dialog;
    }

    public virtual Dialog Build(AppCompatActivity activity, ActionSheetConfig config)
    {
        var builder = new AppCompatAlertDialog.Builder(activity);

        if (config.FontFamily is not null)
        {
            _typeface = Typeface.CreateFromAsset(activity.Assets, config.FontFamily);
        }

        if (config.Title is not null) builder.SetTitle(GetTitle(config));

        if (config.Icon is not null) builder.SetIcon(GetIcon(config));

        builder.SetAdapter(GetActionsAdapter(activity, config), (s, a) => config.Options[a.Which].Action?.Invoke());

        if (config.Destructive is not null)
        {
            builder.SetNegativeButton(GetDestructiveButton(config), (o, a) => config.Destructive.Action?.Invoke());
        }

        if (config.Cancel is not null)
        {
            builder.SetNeutralButton(GetCancelButton(config), (o, a) => config.Cancel.Action?.Invoke());
        }

        var dialog = builder.Create();

        if (config.BackgroundColor is not null)
        {
            dialog.Window.SetBackgroundDrawable(GetDialogBackground(config));
        }

        return dialog;
    }

    protected virtual ActionSheetListAdapter GetActionsAdapter(Android.Content.Context context, ActionSheetConfig config)
    {
        return new ActionSheetListAdapter(context, Android.Resource.Layout.SelectDialogItem, Android.Resource.Id.Text1, this, config, _typeface);
    }

    protected virtual Drawable GetDialogBackground(ActionSheetConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(config.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(config.CornerRadius));

        var draw = new InsetDrawable(backgroundDrawable, DpToPixels(ScreenMargin.Left), DpToPixels(ScreenMargin.Top), DpToPixels(ScreenMargin.Right), DpToPixels(ScreenMargin.Bottom));

        return draw;
    }

    protected virtual SpannableString GetMessage(ActionSheetConfig config)
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

    protected virtual SpannableString GetTitle(ActionSheetConfig config)
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

    protected virtual Drawable GetIcon(ActionSheetConfig config)
    {
        var imgId = MauiApplication.Current.GetDrawableId(config.Icon);
        var img = MauiApplication.Current.GetDrawable(imgId);

        return img;
    }

    protected virtual SpannableString GetCancelButton(ActionSheetConfig config)
    {
        var buttonSpan = new SpannableString(config.Cancel.Text);

        if (config.NegativeButtonTextColor is not null)
        {
            buttonSpan.SetSpan(new ForegroundColorSpan(config.NegativeButtonTextColor.ToPlatform()), 0, config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);
        }
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)config.NegativeButtonFontSize, true), 0, config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);
        if (config.FontFamily is not null)
        {
            buttonSpan.SetSpan(new CustomTypeFaceSpan(_typeface), 0, config.Cancel.Text.Length, SpanTypes.ExclusiveExclusive);
        }

        return buttonSpan;
    }

    protected virtual SpannableString GetDestructiveButton(ActionSheetConfig config)
    {
        var buttonSpan = new SpannableString(config.Destructive.Text);

        if (config.DestructiveButtonTextColor is not null)
        {
            buttonSpan.SetSpan(new ForegroundColorSpan(config.DestructiveButtonTextColor.ToPlatform()), 0, config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);
        }
        buttonSpan.SetSpan(new AbsoluteSizeSpan((int)config.DestructiveButtonFontSize, true), 0, config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);
        buttonSpan.SetSpan(new LetterSpacingSpan(0), 0, config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);
        if (config.FontFamily is not null)
        {
            buttonSpan.SetSpan(new CustomTypeFaceSpan(_typeface), 0, config.Destructive.Text.Length, SpanTypes.ExclusiveExclusive);
        }

        return buttonSpan;
    }
}