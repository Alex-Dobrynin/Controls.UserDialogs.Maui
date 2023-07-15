using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;

using AndroidHUD;

using Google.Android.Material.Snackbar;

using Microsoft.Maui.Platform;

using static Maui.Controls.UserDialogs.Extensions;

namespace Maui.Controls.UserDialogs;

public class SnackbarBuilder
{
    public virtual Snackbar Build(Activity activity, SnackbarConfig config)
    {
        var view = activity.Window.DecorView.RootView.FindViewById(Android.Resource.Id.Content);

        var snackbar = Snackbar.Make(
            activity,
            view,
            config.Message,
            (int)config.Duration.TotalMilliseconds
        );

        snackbar.SetTextColor(SnackbarConfig.MessageColor.ToInt());
        SetupSnackbarText(snackbar, config);

        if (config.Action?.Action != null)
        {
            SetupSnacbarAction(activity, snackbar, config);
        }

        snackbar.View.Background = GetDialogBackground(config);

        if (snackbar.View.LayoutParameters is FrameLayout.LayoutParams layoutParams)
        {
            layoutParams.SetMargins(DpToPixels(20), DpToPixels(50), DpToPixels(20), DpToPixels(30));
            layoutParams.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Bottom;

            if (SnackbarConfig.Position == ToastPosition.Top)
            {
                layoutParams.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Top;
            }

            snackbar.View.LayoutParameters = layoutParams;
        }

        snackbar.SetAnimationMode(Snackbar.AnimationModeFade);

        return snackbar;
    }

    protected virtual Drawable GetDialogBackground(SnackbarConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(SnackbarConfig.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(SnackbarConfig.CornerRadius));

        return backgroundDrawable;
    }

    protected virtual void SetupSnackbarText(Snackbar snackbar, SnackbarConfig config)
    {
        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;
        var text = l.GetChildAt(0) as TextView;
        text.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)SnackbarConfig.MessageFontSize);

        if (config.Icon == null) return;

        var icon = GetIcon(config.Icon);
        icon.ScaleTo(22);
        text.SetCompoundDrawables(icon, null, null, null);
        text.CompoundDrawablePadding = DpToPixels(10);
    }

    protected virtual void SetupSnacbarAction(Activity activity, Snackbar snackbar, SnackbarConfig config)
    {
        CountDownTimer cd = null;

        if (config.Action.ShowCountDown is true)
        {
            cd = PrepareProgress(activity, snackbar, config);
        }

        var text = new SpannableString(config.Action.Text);
        text.SetSpan(new LetterSpacingSpan(0), 0, config.Action.Text.Length, SpanTypes.ExclusiveExclusive);

        snackbar.SetActionTextColor(SnackbarConfig.PositiveButtonTextColor.ToInt());
        snackbar.SetAction(text, v =>
        {
            cd?.Cancel();
            config.Action.Action();
        });

        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;
        var button = l.GetChildAt(1) as Android.Widget.Button;
        button.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)SnackbarConfig.PositiveButtonFontSize);

        if (config.Action.Icon == null) return;

        var icon = GetIcon(config.Action.Icon);
        icon.ScaleTo(22);
        button.SetCompoundDrawables(icon, null, null, null);
    }

    protected virtual Drawable GetIcon(string icon)
    {
        var imgId = MauiApplication.Current.GetDrawableId(icon);
        var img = MauiApplication.Current.GetDrawable(imgId);

        return img;
    }

    protected virtual CountDownTimer PrepareProgress(Activity activity, Snackbar snackbar, SnackbarConfig config)
    {
        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;

        var lParams = new LinearLayout.LayoutParams(DpToPixels(24), DpToPixels(24));
        lParams.SetMargins(0, 0, 0, 0);
        lParams.Gravity = GravityFlags.Center;

        var wheel = new ProgressWheel(activity)
        {
            RimWidth = DpToPixels(2),
            BarWidth = DpToPixels(2),
            RimColor = Android.Graphics.Color.Transparent,
            BarColor = SnackbarConfig.PositiveButtonTextColor.ToPlatform(),
            LayoutParameters = lParams
        };

        wheel.SetPadding(0, 0, 0, 0);

        l.AddView(wheel);

        return new CountDown((long)config.Duration.TotalMilliseconds, 200, wheel).Start();
    }

    private class CountDown : CountDownTimer
    {
        private readonly long _millisInFuture;
        private readonly ProgressWheel _wheel;

        public CountDown(long millisInFuture, long countDownInterval, ProgressWheel wheel)
            : base(millisInFuture, countDownInterval)
        {
            _millisInFuture = millisInFuture;
            _wheel = wheel;
        }

        public override void OnFinish()
        {
            _wheel.SetProgress(0);
        }

        public override void OnTick(long millisUntilFinished)
        {
            var percent = (double)millisUntilFinished / _millisInFuture * 100;

            _wheel.SetProgress((int)percent);
        }
    }
}