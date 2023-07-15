using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

using Google.Android.Material.Snackbar;

using Microsoft.Maui.Platform;

using static Maui.Controls.UserDialogs.Extensions;

namespace Maui.Controls.UserDialogs;

public class ToastBuilder
{
    public virtual Snackbar Build(Activity activity, ToastConfig config)
    {
        var view = activity.Window.DecorView.RootView.FindViewById(Android.Resource.Id.Content);

        var snackBar = Snackbar.Make(
            activity,
            view,
            config.Message,
            (int)config.Duration.TotalMilliseconds
        );

        SetupSnackbarText(snackBar, config);

        snackBar.View.Background = GetDialogBackground(config);

        if (snackBar.View.LayoutParameters is FrameLayout.LayoutParams layoutParams)
        {
            layoutParams.Height = FrameLayout.LayoutParams.WrapContent;
            layoutParams.Width = FrameLayout.LayoutParams.WrapContent;
            layoutParams.SetMargins(0, DpToPixels(50), 0, DpToPixels(80));
            layoutParams.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Bottom;

            if (ToastConfig.Position == ToastPosition.Top)
            {
                layoutParams.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Top;
            }

            snackBar.View.LayoutParameters = layoutParams;
        }

        snackBar.SetAnimationMode(Snackbar.AnimationModeFade);

        snackBar.SetTextColor(ToastConfig.MessageColor.ToInt());

        return snackBar;
    }

    protected virtual Drawable GetDialogBackground(ToastConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(ToastConfig.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(ToastConfig.CornerRadius));

        return backgroundDrawable;
    }

    protected virtual void SetupSnackbarText(Snackbar snackbar, ToastConfig config)
    {
        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;
        var text = l.GetChildAt(0) as TextView;
        text.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)SnackbarConfig.MessageFontSize);

        if (config.Icon == null) return;

        var icon = GetIcon(config);
        icon.ScaleTo(22);
        text.SetCompoundDrawables(icon, null, null, null);
        text.CompoundDrawablePadding = DpToPixels(10);
    }

    protected virtual Drawable GetIcon(ToastConfig config)
    {
        var imgId = MauiApplication.Current.GetDrawableId(config.Icon);
        var img = MauiApplication.Current.GetDrawable(imgId);

        return img;
    }
}