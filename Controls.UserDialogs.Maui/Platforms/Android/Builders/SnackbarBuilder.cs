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


    private Typeface _typeface;
    private Action _dismissed;
    private Activity _activity;
    private SnackbarConfig _config;

    public SnackbarBuilder(Activity activity, SnackbarConfig config)
    {
        _activity = activity;
        _config = config;
    }

    public override void OnShown(Snackbar snackbar)
    {
        base.OnShown(snackbar);

        var timer = new System.Timers.Timer();
        timer.Interval = _config.Duration.TotalMilliseconds - FadeInFadeOutAnimationDuration;
        timer.AutoReset = false;
        timer.Elapsed += (s, a) =>
        {
            _activity.RunOnUiThread(() =>
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

        if (_config.Action is not null)
        {
            if (e == Snackbar.Callback.DismissEventTimeout)
            {
                _config.Action(SnackbarActionType.Timeout);
            }
            else if (e == Snackbar.Callback.DismissEventConsecutive)
            {
                _config.Action(SnackbarActionType.Cancelled);
            }
        }
    }

    public virtual Snackbar Build()
    {
        if (_config.FontFamily is not null)
        {
            _typeface = Typeface.CreateFromAsset(_activity.Assets, _config.FontFamily);
        }

        var view = _activity.Window.DecorView.RootView.FindViewById(Android.Resource.Id.Content);

        var snackbar = Snackbar.Make(
            _activity,
            view,
            _config.Message,
            (int)_config.Duration.TotalMilliseconds
        );

        SetupSnackbarText(snackbar, _config);
        if (_config.MessageColor is not null)
        {
            snackbar.SetTextColor(_config.MessageColor.ToInt());
        }

        if (_config.Action is not null)
        {
            SetupSnackbarAction(_activity, snackbar, _config);
        }

        if (_config.BackgroundColor is not null)
        {
            snackbar.View.Background = GetDialogBackground(_config);
        }

        if (snackbar.View.LayoutParameters is FrameLayout.LayoutParams layoutParams)
        {
            layoutParams.SetMargins(DpToPixels(ScreenMargin.Left), DpToPixels(ScreenMargin.Top), DpToPixels(ScreenMargin.Right), DpToPixels(ScreenMargin.Bottom));
            layoutParams.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Bottom;

            if (_config.Position == ToastPosition.Top)
            {
                layoutParams.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Top;
            }

            snackbar.View.LayoutParameters = layoutParams;
        }

        snackbar.AddCallback(this);

        snackbar.View.Alpha = 0f;

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
        icon.ScaleTo(IconSize);
        text.SetCompoundDrawables(icon, null, null, null);
        text.CompoundDrawablePadding = DpToPixels(IconPadding);
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

    protected virtual CountDownTimer PrepareProgress(Activity activity, Snackbar snackbar, SnackbarConfig config)
    {
        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;

        var lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent)
        {
            Gravity = GravityFlags.Center
        };

        var text = new TextView(activity);
        text.SetTypeface(_typeface, TypefaceStyle.Normal);
        if (config.PositiveButtonTextColor is not null)
        {
            text.SetTextColor(config.PositiveButtonTextColor.ToPlatform());
        }
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
            _text.Text = "" + Math.Round(millisUntilFinished / 1000.0);
        }
    }
}