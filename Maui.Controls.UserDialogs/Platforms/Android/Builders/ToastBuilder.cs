using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

using Google.Android.Material.Snackbar;

using Microsoft.Maui.Platform;

using static Maui.Controls.UserDialogs.Extensions;

namespace Maui.Controls.UserDialogs;

public class ToastBuilder : Snackbar.Callback
{
    public static Thickness DefaultScreenMargin { get; set; } = new Thickness(20, 50, 20, 80);
    public static double DefaultIconPadding { get; set; } = 10;
    public static double DefaultIconSize { get; set; } = 24;
    public static long DefaultFadeInFadeOutAnimationDuration { get; set; } = 300;

    public Thickness ScreenMargin { get; set; } = DefaultScreenMargin;
    public double IconPadding { get; set; } = DefaultIconPadding;
    public double IconSize { get; set; } = DefaultIconSize;
    public long FadeInFadeOutAnimationDuration { get; set; } = DefaultFadeInFadeOutAnimationDuration;

    private Typeface _typeface;

    private Action _dismissed;
    private readonly Activity _activity;
    private readonly ToastConfig _config;

    public ToastBuilder(Activity activity, ToastConfig config)
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

        if (_config.BackgroundColor is not null)
        {
            snackbar.View.Background = GetDialogBackground(_config);
        }

        if (snackbar.View.LayoutParameters is FrameLayout.LayoutParams layoutParams)
        {
            layoutParams.Height = FrameLayout.LayoutParams.WrapContent;
            layoutParams.Width = FrameLayout.LayoutParams.WrapContent;
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

    protected virtual Drawable GetDialogBackground(ToastConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(config.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(config.CornerRadius));

        return backgroundDrawable;
    }

    protected virtual void SetupSnackbarText(Snackbar snackbar, ToastConfig config)
    {
        var l = (snackbar.View as Snackbar.SnackbarLayout).GetChildAt(0) as SnackbarContentLayout;
        var text = l.GetChildAt(0) as TextView;
        text.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)config.MessageFontSize);
        text.SetTypeface(_typeface, TypefaceStyle.Normal);
        text.SetMaxLines(int.MaxValue);

        if (config.Icon is null) return;

        var icon = GetIcon(config);

        text.SetCompoundDrawables(icon, null, null, null);
        text.CompoundDrawablePadding = DpToPixels(IconPadding);
    }

    protected virtual Drawable GetIcon(ToastConfig config)
    {
        var imgId = MauiApplication.Current.GetDrawableId(config.Icon);
        var img = MauiApplication.Current.GetDrawable(imgId);
        img.ScaleTo(IconSize);

        return img;
    }
}