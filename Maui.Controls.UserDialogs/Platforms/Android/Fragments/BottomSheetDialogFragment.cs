using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;

using Google.Android.Material.BottomSheet;

using Microsoft.Maui.Platform;

using static Maui.Controls.UserDialogs.Extensions;

using Orientation = Android.Widget.Orientation;
using View = Android.Views.View;

namespace Maui.Controls.UserDialogs;

public class BottomSheetDialogFragment : AbstractAppCompatDialogFragment<ActionSheetConfig>
{
    protected override void SetDialogDefaults(Dialog dialog)
    {
        base.SetDialogDefaults(dialog);

        dialog.CancelEvent += (sender, args) => this.Config?.Cancel?.Action?.Invoke();

        var cancellable = this.Config.Cancel is not null;
        dialog.SetCancelable(cancellable);
        dialog.SetCanceledOnTouchOutside(cancellable);
    }

    protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
    {
        base.OnKeyPress(sender, args);

        if (args.KeyCode != Keycode.Back)
            return;

        args.Handled = true;
        this.Config?.Cancel?.Action?.Invoke();
        this.Dismiss();
    }

    protected override Dialog CreateDialog(ActionSheetConfig config)
    {
        var dialog = new BottomSheetDialog(this.Activity);

        var layout = new LinearLayout(this.Activity)
        {
            Orientation = Orientation.Vertical
        };

        if (config.Title is not null)
            layout.AddView(this.GetHeaderText());

        if (config.Message is not null)
            layout.AddView(this.GetMessageText());

        foreach (var action in config.Options)
            layout.AddView(this.CreateActionRow(action));

        if (config.Destructive is not null)
        {
            layout.AddView(this.CreateDivider());
            layout.AddView(this.CreateDestructiveRow(config.Destructive));
        }
        if (config.Cancel is not null)
        {
            layout.AddView(this.CreateDivider());
            layout.AddView(this.CreateCancelRow(config.Cancel));
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
        backgroundDrawable.SetColor(Config.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(Config.CornerRadius));

        return backgroundDrawable;
    }

    protected virtual TextView GetHeaderText()
    {
        var lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
        lParams.SetMargins(DpToPixels(24), DpToPixels(18), DpToPixels(24), 0);
        var textView = new TextView(this.Activity)
        {
            Text = Config.Title,
            LayoutParameters = lParams,
            Gravity = GravityFlags.CenterVertical
        };
        textView.SetTextSize(ComplexUnitType.Sp, (float)Config.TitleFontSize);
        textView.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);
        if (Config.TitleColor is not null)
        {
            textView.SetTextColor(Config.TitleColor.ToPlatform());
        }

        if (Config.Icon is not null)
        {
            textView.SetCompoundDrawables(GetDialogIcon(), null, null, null);

            textView.CompoundDrawablePadding = DpToPixels(10);
        }

        return textView;
    }

    protected virtual Drawable GetDialogIcon()
    {
        var imgId = MauiApplication.Current.GetDrawableId(Config.Icon);
        var img = MauiApplication.Current.GetDrawable(imgId);
        img.ScaleTo(36);

        return img;
    }

    protected virtual TextView GetMessageText()
    {
        var lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
        lParams.SetMargins(DpToPixels(24), 0, DpToPixels(24), 0);
        var textView = new TextView(this.Activity)
        {
            Text = Config.Message,
            LayoutParameters = lParams
        };
        textView.SetTextSize(ComplexUnitType.Sp, (float)Config.MessageFontSize);
        if (Config.MessageColor is not null)
        {
            textView.SetTextColor(Config.MessageColor.ToPlatform());
        }

        return textView;
    }

    protected virtual View CreateDestructiveRow(ActionSheetOption action)
    {
        var row = new LinearLayout(this.Activity)
        {
            Clickable = true,
            Orientation = Orientation.Horizontal,
            LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, DpToPixels(50)),
            Foreground = Extensions.GetSelectableItemForeground(this.Activity)
        };

        row.AddView(this.GetActionText(action, Config.DestructiveButtonTextColor, Config.DestructiveButtonFontSize));
        row.Click += (sender, args) =>
        {
            action.Action?.Invoke();
            this.Dismiss();
        };
        return row;
    }

    protected virtual View CreateCancelRow(ActionSheetOption action)
    {
        var row = new LinearLayout(this.Activity)
        {
            Clickable = true,
            Orientation = Orientation.Horizontal,
            LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, DpToPixels(50)),
            Foreground = Extensions.GetSelectableItemForeground(this.Activity)
        };

        row.AddView(this.GetActionText(action, Config.NegativeButtonTextColor, Config.NegativeButtonFontSize));
        row.Click += (sender, args) =>
        {
            action.Action?.Invoke();
            this.Dismiss();
        };
        return row;
    }

    protected virtual View CreateActionRow(ActionSheetOption action)
    {
        var row = new LinearLayout(this.Activity)
        {
            Clickable = true,
            Orientation = Orientation.Horizontal,
            LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, DpToPixels(50)),
            Foreground = Extensions.GetSelectableItemForeground(this.Activity)
        };

        row.AddView(this.GetActionText(action, Config.ActionSheetOptionTextColor, Config.ActionSheetOptionFontSize));
        row.Click += (sender, args) =>
        {
            action.Action?.Invoke();
            this.Dismiss();
        };
        return row;
    }

    protected virtual TextView GetActionText(ActionSheetOption action, Color color, double fontSize)
    {
        var lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);

        lParams.SetMargins(DpToPixels(20), 0, DpToPixels(20), 0);

        var textView = new TextView(this.Activity)
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

        if (action.Icon is not null)
        {
            textView.SetCompoundDrawables(GetActionIcon(action), null, null, null);

            textView.CompoundDrawablePadding = DpToPixels(10);
        }

        return textView;
    }

    protected virtual Drawable GetActionIcon(ActionSheetOption action)
    {
        var imgId = MauiApplication.Current.GetDrawableId(action.Icon);
        var img = MauiApplication.Current.GetDrawable(imgId);
        img.ScaleTo(24);

        return img;
    }

    protected virtual View CreateDivider()
    {
        var view = new View(this.Activity)
        {
            Background = new ColorDrawable(Config.ActionSheetSeparatorColor.ToPlatform()),
            LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, DpToPixels(1))
        };
        view.SetPadding(0, DpToPixels(7), 0, DpToPixels(8));
        return view;
    }
}