using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;

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

        if (config.Action is not null)
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

        if (config.ShowCountDown)
        {
            cd = PrepareProgress(activity, snackbar, config);
        }

        var text = new SpannableString(config.ActionText);
        text.SetSpan(new LetterSpacingSpan(0), 0, config.ActionText.Length, SpanTypes.ExclusiveExclusive);

        if (config.PositiveButtonTextColor is not null)
        {
            snackbar.SetActionTextColor(config.PositiveButtonTextColor.ToInt());
        }
        snackbar.SetAction(text, v =>
        {
            cd?.Cancel();
            config.Action?.Invoke(SnackbarActionType.UserInteraction);
        });

        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;
        var button = l.GetChildAt(1) as Android.Widget.Button;
        button.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)config.PositiveButtonFontSize);
        button.SetTypeface(_typeface, TypefaceStyle.Normal);

        if (config.Icon is null) return;

        var icon = GetIcon(config.Icon);
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

        var lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent)
        {
            Gravity = GravityFlags.Center
        };

        var text = new TextView(activity);
        text.SetTypeface(_typeface, TypefaceStyle.Normal);
        text.SetTextColor(config.PositiveButtonTextColor.ToPlatform());
        text.Text = "" + Math.Round(config.Duration.TotalSeconds);
        text.LayoutParameters = lParams;

        l.AddView(text);

        return new CountDown((long)config.Duration.TotalMilliseconds, 500, text).Start();
    }

    private class CountDown : CountDownTimer
    {
        private readonly TextView _text;

        public CountDown(long millisInFuture, long countDownInterval, TextView text)
            : base(millisInFuture, countDownInterval)
        {
            _text = text;
        }

        public override void OnFinish()
        {
            _text.Text = "0";
        }

        public override void OnTick(long millisUntilFinished)
        {
            _text.Text = "" + Math.Round(millisUntilFinished * 1000.0);
        }
    }
}