using Android.App;
using Android.Graphics;
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
    private Typeface _typeface;

    public virtual Snackbar Build(Activity activity, SnackbarConfig config)
    {
        if (config.FontFamily is not null)
        {
            _typeface = Typeface.CreateFromAsset(activity.Assets, config.FontFamily);
        }

        var view = activity.Window.DecorView.RootView.FindViewById(Android.Resource.Id.Content);

        var snackbar = Snackbar.Make(
            activity,
            view,
            config.Message,
            (int)config.Duration.TotalMilliseconds
        );

        SetupSnackbarText(snackbar, config);
        if (config.MessageColor is not null)
        {
            snackbar.SetTextColor(config.MessageColor.ToInt());
        }

        if (config.Action?.Action is not null)
        {
            SetupSnackbarAction(activity, snackbar, config);
        }

        if (config.BackgroundColor is not null)
        {
            snackbar.View.Background = GetDialogBackground(config);
        }

        if (snackbar.View.LayoutParameters is FrameLayout.LayoutParams layoutParams)
        {
            layoutParams.SetMargins(DpToPixels(20), DpToPixels(50), DpToPixels(20), DpToPixels(30));
            layoutParams.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Bottom;

            if (config.Position == ToastPosition.Top)
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
        backgroundDrawable.SetColor(config.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(config.CornerRadius));

        return backgroundDrawable;
    }

    protected virtual void SetupSnackbarText(Snackbar snackbar, SnackbarConfig config)
    {
        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;
        var text = l.GetChildAt(0) as TextView;
        text.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)config.MessageFontSize);
        text.SetTypeface(_typeface, TypefaceStyle.Normal);

        if (config.Icon is null) return;

        var icon = GetIcon(config.Icon);
        icon.ScaleTo(22);
        text.SetCompoundDrawables(icon, null, null, null);
        text.CompoundDrawablePadding = DpToPixels(10);
    }

    protected virtual void SetupSnackbarAction(Activity activity, Snackbar snackbar, SnackbarConfig config)
    {
        CountDownTimer cd = null;

        if (config.Action.ShowCountDown is true)
        {
            cd = PrepareProgress(activity, snackbar, config);
        }

        var text = new SpannableString(config.Action.Text);
        text.SetSpan(new LetterSpacingSpan(0), 0, config.Action.Text.Length, SpanTypes.ExclusiveExclusive);

        if (config.PositiveButtonTextColor is not null)
        {
            snackbar.SetActionTextColor(config.PositiveButtonTextColor.ToInt());
        }
        snackbar.SetAction(text, v =>
        {
            cd?.Cancel();
            config.Action.Action();
        });

        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;
        var button = l.GetChildAt(1) as Android.Widget.Button;
        button.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)config.PositiveButtonFontSize);
        button.SetTypeface(_typeface, TypefaceStyle.Normal);

        if (config.Action.Icon is null) return;

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
            BarColor = (config.PositiveButtonTextColor ?? Colors.White).ToPlatform(),
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