using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;

using Google.Android.Material.BottomSheet;

using Microsoft.Maui.Platform;

using static Controls.UserDialogs.Maui.Extensions;

using Color = Microsoft.Maui.Graphics.Color;
using Orientation = Android.Widget.Orientation;
using View = Android.Views.View;

namespace Controls.UserDialogs.Maui;

public class BottomSheetDialogFragment : AbstractAppCompatDialogFragment<ActionSheetConfig>
{
    public static Thickness DefaultScreenMargin { get; set; } = new Thickness(0);
    public static Thickness DefaultPadding { get; set; } = new Thickness(24, 18);
    public static double DefaultIconPadding { get; set; } = 10;
    public static double DefaultOptionIconPadding { get; set; } = 10;
    public static double DefaultOptionIconSize { get; set; } = 24;
    public static double DefaultIconSize { get; set; } = 36;

    public Thickness ScreenMargin { get; set; } = DefaultScreenMargin;
    public Thickness Padding { get; set; } = DefaultPadding;
    public double IconPadding { get; set; } = DefaultIconPadding;
    public double OptionIconPadding { get; set; } = DefaultOptionIconPadding;
    public double OptionIconSize { get; set; } = DefaultOptionIconSize;
    public double IconSize { get; set; } = DefaultIconSize;

    protected override void SetDialogDefaults(Dialog dialog)
    {
        base.SetDialogDefaults(dialog);

        dialog.CancelEvent += (sender, args) => Config?.Cancel?.Action?.Invoke();

        var cancellable = Config?.Cancel is not null;
        dialog.SetCancelable(cancellable);
        dialog.SetCanceledOnTouchOutside(cancellable);
    }

    public override void OnCancel(IDialogInterface dialog)
    {
        base.OnCancel(dialog);
        Config?.Cancel?.Action?.Invoke();
    }

    protected override void OnKeyPress(object? sender, DialogKeyEventArgs args)
    {
        base.OnKeyPress(sender, args);

        if (args.KeyCode != Keycode.Back)
            return;

        args.Handled = true;
        Config?.Cancel?.Action?.Invoke();
        Dismiss();
    }

    protected override Dialog CreateDialog(ActionSheetConfig config)
    {
        var dialog = new BottomSheetDialog(Activity!);

        var layout = new LinearLayout(Activity)
        {
            Orientation = Orientation.Vertical
        };

        if (config.Title is not null)
        {
            layout.AddView(GetHeaderText());
        }

        if (config.Message is not null)
        {
            layout.AddView(GetMessageText());
        }

        foreach (var action in config.Options)
        {
            layout.AddView(CreateActionRow(action));
        }

        if (config.Destructive is not null)
        {
            layout.AddView(CreateDivider());
            layout.AddView(CreateDestructiveRow(config.Destructive));
        }

        if (config.Cancel is not null)
        {
            layout.AddView(CreateDivider());
            layout.AddView(CreateCancelRow(config.Cancel));
        }

        if (config.BackgroundColor is not null)
        {
            layout.Background = GetDialogBackground();
        }

        dialog.SetContentView(layout);

        return dialog;
    }

    protected virtual Drawable GetDialogBackground()
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(Config!.BackgroundColor!.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(Config.CornerRadius));

        var draw = new InsetDrawable(backgroundDrawable, DpToPixels(ScreenMargin.Left), DpToPixels(ScreenMargin.Top), DpToPixels(ScreenMargin.Right), DpToPixels(ScreenMargin.Bottom));

        return draw;
    }

    protected virtual TextView GetHeaderText()
    {
        var lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
        lParams.SetMargins(DpToPixels(Padding.Left), DpToPixels(Padding.Top), DpToPixels(Padding.Right), 0);
        var textView = new TextView(Activity)
        {
            Text = Config!.Title,
            LayoutParameters = lParams,
            Gravity = GravityFlags.CenterVertical
        };
        textView.SetTextSize(ComplexUnitType.Sp, (float)Config.TitleFontSize);

        if (Config.TitleFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity!.Assets, Config.TitleFontFamily);
            textView.SetTypeface(typeface, TypefaceStyle.Bold);
        }

        if (Config.TitleColor is not null)
        {
            textView.SetTextColor(Config.TitleColor.ToPlatform());
        }

        if (Config.Icon is not null)
        {
            textView.SetCompoundDrawables(GetDialogIcon(), null, null, null);

            textView.CompoundDrawablePadding = DpToPixels(IconPadding);
        }

        return textView;
    }

    protected virtual Drawable GetDialogIcon()
    {
        var imgId = MauiApplication.Current.GetDrawableId(Config!.Icon!);
        var img = MauiApplication.Current.GetDrawable(imgId)!;
        img.ScaleTo(IconSize);

        return img;
    }

    protected virtual TextView GetMessageText()
    {
        var lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
        lParams.SetMargins(DpToPixels(Padding.Left), 0, DpToPixels(Padding.Right), 0);
        var textView = new TextView(Activity)
        {
            Text = Config!.Message,
            LayoutParameters = lParams
        };
        textView.SetTextSize(ComplexUnitType.Sp, (float)Config.MessageFontSize);
        if (Config.MessageColor is not null)
        {
            textView.SetTextColor(Config.MessageColor.ToPlatform());
        }

        if (Config.MessageFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity!.Assets, Config.MessageFontFamily);
            textView.SetTypeface(typeface, TypefaceStyle.Normal);
        }

        return textView;
    }

    protected virtual View CreateDestructiveRow(ActionSheetOption action)
    {
        var row = new LinearLayout(Activity)
        {
            Clickable = true,
            Orientation = Orientation.Horizontal,
            LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, DpToPixels(50)),
            Foreground = GetSelectableItemForeground(Activity!)
        };

        row.AddView(GetActionText(action, Config!.DestructiveButtonFontSize, Config.DestructiveButtonTextColor, Config.DestructiveButtonFontFamily));
        row.Click += (sender, args) =>
        {
            action.Action?.Invoke();
            Dismiss();
        };
        return row;
    }

    protected virtual View CreateCancelRow(ActionSheetOption action)
    {
        var row = new LinearLayout(Activity)
        {
            Clickable = true,
            Orientation = Orientation.Horizontal,
            LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, DpToPixels(50)),
            Foreground = GetSelectableItemForeground(Activity!)
        };

        row.AddView(GetActionText(action, Config!.NegativeButtonFontSize, Config.NegativeButtonTextColor, Config.NegativeButtonFontFamily));
        row.Click += (sender, args) =>
        {
            action.Action?.Invoke();
            Dismiss();
        };
        return row;
    }

    protected virtual View CreateActionRow(ActionSheetOption action)
    {
        var row = new LinearLayout(Activity)
        {
            Clickable = true,
            Orientation = Orientation.Horizontal,
            LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, DpToPixels(50)),
            Foreground = GetSelectableItemForeground(Activity!)
        };

        row.AddView(GetActionText(action, Config!.ActionSheetOptionFontSize, Config.ActionSheetOptionTextColor, Config.OptionsButtonFontFamily));
        row.Click += (sender, args) =>
        {
            action.Action?.Invoke();
            Dismiss();
        };
        return row;
    }

    protected virtual TextView GetActionText(ActionSheetOption action, double fontSize, Color? color = null, string? fontFamily = null)
    {
        var lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);

        lParams.SetMargins(DpToPixels(Padding.Left), 0, DpToPixels(Padding.Right), 0);

        var textView = new TextView(Activity)
        {
            Text = action.Text,
            LayoutParameters = lParams,
            Gravity = GravityFlags.CenterVertical
        };
        textView.SetTextSize(ComplexUnitType.Sp, (int)fontSize);

        if (color is not null)
        {
            textView.SetTextColor(color.ToPlatform());
        }

        if (fontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity!.Assets, fontFamily);
            textView.SetTypeface(typeface, TypefaceStyle.Normal);
        }

        if (action.Icon is not null)
        {
            textView.SetCompoundDrawables(GetActionIcon(action), null, null, null);

            textView.CompoundDrawablePadding = DpToPixels(OptionIconPadding);
        }

        return textView;
    }

    protected virtual Drawable GetActionIcon(ActionSheetOption action)
    {
        var imgId = MauiApplication.Current.GetDrawableId(action.Icon!);
        var img = MauiApplication.Current.GetDrawable(imgId)!;
        img.ScaleTo(OptionIconSize);

        return img;
    }

    protected virtual View CreateDivider()
    {
        var view = new View(Activity)
        {
            Background = new ColorDrawable(Config!.ActionSheetSeparatorColor.ToPlatform()),
            LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, DpToPixels(1))
        };
        view.SetPadding(0, DpToPixels(7), 0, DpToPixels(8));
        return view;
    }
}