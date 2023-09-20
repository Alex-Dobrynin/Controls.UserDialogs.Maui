using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;

using Google.Android.Material.Snackbar;

using Microsoft.Maui.Platform;

using static Controls.UserDialogs.Maui.Extensions;

namespace Controls.UserDialogs.Maui;

public class SnackbarBuilder : Snackbar.Callback
{
    public static Thickness DefaultScreenMargin { get; set; } = new Thickness(20, 50, 20, 30);
    public static double DefaultIconPadding { get; set; } = 10;
    public static double DefaultActionIconPadding { get; set; } = 10;
    public static double DefaultActionIconSize { get; set; } = 22;
    public static double DefaultIconSize { get; set; } = 24;
    public static long DefaultFadeInFadeOutAnimationDuration { get; set; } = 300;


    public Thickness ScreenMargin { get; set; } = DefaultScreenMargin;
    public double IconPadding { get; set; } = DefaultIconPadding;
    public double IconSize { get; set; } = DefaultIconSize;
    public double ActionIconSize { get; set; } = DefaultActionIconSize;
    public double ActionIconPadding { get; set; } = DefaultActionIconPadding;
    public long FadeInFadeOutAnimationDuration { get; set; } = DefaultFadeInFadeOutAnimationDuration;

    private Action _dismissed;

    protected Activity Activity { get; }
    protected SnackbarConfig Config { get; }

    public SnackbarBuilder(Activity activity, SnackbarConfig config)
    {
        Activity = activity;
        Config = config;
    }

    public override void OnShown(Snackbar snackbar)
    {
        base.OnShown(snackbar);

        var timer = new System.Timers.Timer
        {
            Interval = Config.Duration.TotalMilliseconds,
            AutoReset = false
        };
        timer.Elapsed += (s, a) =>
        {
            Activity.RunOnUiThread(() =>
            {
                snackbar.View.Animate().Alpha(0f).SetDuration(FadeInFadeOutAnimationDuration).Start();
            });
        };
        timer.Start();

        _dismissed = () =>
        {
            try
            {
                timer.Stop();
            }
            catch { }
        };

        snackbar.View.Animate().Alpha(1f).SetDuration(FadeInFadeOutAnimationDuration).Start();
    }

    public override void OnDismissed(Snackbar snackbar, int e)
    {
        base.OnDismissed(snackbar, e);

        _dismissed?.Invoke();

        if (Config.Action is not null)
        {
            if (e == Snackbar.Callback.DismissEventTimeout)
            {
                Config.Action(SnackbarActionType.Timeout);
            }
            else if (e == Snackbar.Callback.DismissEventConsecutive)
            {
                Config.Action(SnackbarActionType.Cancelled);
            }
        }
    }

    public virtual Snackbar Build()
    {
        var snackbar = Snackbar.Make(
            Activity,
            Activity.Window.DecorView,
            Config.Message,
            (int)Config.Duration.TotalMilliseconds
        );

        SetupSnackbarText(snackbar);

        if (Config.MessageColor is not null)
        {
            snackbar.SetTextColor(Config.MessageColor.ToInt());
        }

        if (Config.Action is not null)
        {
            SetupSnackbarAction(snackbar);
        }

        if (Config.BackgroundColor is not null)
        {
            snackbar.View.Background = GetDialogBackground();
        }

        if (snackbar.View.LayoutParameters is FrameLayout.LayoutParams layoutParams)
        {
            layoutParams.SetMargins(DpToPixels(ScreenMargin.Left), DpToPixels(ScreenMargin.Top), DpToPixels(ScreenMargin.Right), DpToPixels(ScreenMargin.Bottom));
            layoutParams.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Bottom;

            if (Config.Position == ToastPosition.Top)
            {
                layoutParams.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Top;
            }

            snackbar.View.LayoutParameters = layoutParams;
        }

        snackbar.AddCallback(this);

        snackbar.View.Alpha = 0f;

        return snackbar;
    }

    protected virtual Drawable GetDialogBackground()
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(Config.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(Config.CornerRadius));

        return backgroundDrawable;
    }

    protected virtual void SetupSnackbarText(Snackbar snackbar)
    {
        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;
        var text = l.GetChildAt(0) as TextView;
        text.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)Config.MessageFontSize);

        if (Config.MessageFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity.Assets, Config.MessageFontFamily);
            text.SetTypeface(typeface, TypefaceStyle.Normal);
        }

        if (Config.Icon is null) return;

        var icon = GetIcon(Config.Icon);
        icon.ScaleTo(IconSize);
        text.SetCompoundDrawables(icon, null, null, null);
        text.CompoundDrawablePadding = DpToPixels(IconPadding);
    }

    protected virtual void SetupSnackbarAction(Snackbar snackbar)
    {
        CountDownTimer cd = null;

        if (Config.ShowCountDown)
        {
            cd = PrepareProgress(snackbar);
        }

        var text = new SpannableString(Config.ActionText);
        text.SetSpan(new LetterSpacingSpan(0), 0, Config.ActionText.Length, SpanTypes.ExclusiveExclusive);

        if (Config.NegativeButtonTextColor is not null)
        {
            snackbar.SetActionTextColor(Config.NegativeButtonTextColor.ToInt());
        }
        snackbar.SetAction(text, v =>
        {
            cd?.Cancel();
            Config.Action?.Invoke(SnackbarActionType.UserInteraction);
        });

        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;
        var button = l.GetChildAt(1) as Android.Widget.Button;
        button.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)Config.NegativeButtonFontSize);

        if (Config.NegativeButtonFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity.Assets, Config.NegativeButtonFontFamily);
            button.SetTypeface(typeface, TypefaceStyle.Normal);
        }

        if (Config.Icon is null) return;

        var icon = GetIcon(Config.Icon);
        icon.ScaleTo(ActionIconSize);
        button.SetCompoundDrawables(icon, null, null, null);
        button.CompoundDrawablePadding = DpToPixels(ActionIconPadding);
    }

    protected virtual Drawable GetIcon(string icon)
    {
        var imgId = MauiApplication.Current.GetDrawableId(icon);
        var img = MauiApplication.Current.GetDrawable(imgId);

        return img;
    }

    protected virtual CountDownTimer PrepareProgress(Snackbar snackbar)
    {
        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;

        var lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent)
        {
            Gravity = GravityFlags.Center
        };

        var text = new TextView(Activity);

        if (Config.MessageFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity.Assets, Config.MessageFontFamily);
            text.SetTypeface(typeface, TypefaceStyle.Normal);
        }

        if (Config.NegativeButtonTextColor is not null)
        {
            text.SetTextColor(Config.NegativeButtonTextColor.ToPlatform());
        }
        text.Text = "" + Math.Round(Config.Duration.TotalSeconds);
        text.LayoutParameters = lParams;

        l.AddView(text);

        return new CountDown((long)Config.Duration.TotalMilliseconds, 500, text).Start();
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
            _text.Text = "" + Math.Round(millisUntilFinished / 1000.0);
        }
    }
}