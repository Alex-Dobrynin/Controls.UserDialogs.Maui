using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

using Google.Android.Material.Snackbar;

using Microsoft.Maui.Platform;

using static Controls.UserDialogs.Maui.Extensions;

namespace Controls.UserDialogs.Maui;

public class ToastBuilder : Snackbar.Callback
{
    public static Thickness DefaultScreenMargin { get; set; } = new Thickness(20, 50, 20, 100);
    public static double DefaultIconPadding { get; set; } = 10;
    public static double DefaultIconSize { get; set; } = 24;
    public static long DefaultFadeInFadeOutAnimationDuration { get; set; } = 300;

    public Thickness ScreenMargin { get; set; } = DefaultScreenMargin;
    public double IconPadding { get; set; } = DefaultIconPadding;
    public double IconSize { get; set; } = DefaultIconSize;
    public long FadeInFadeOutAnimationDuration { get; set; } = DefaultFadeInFadeOutAnimationDuration;

    private Action _dismissed;

    protected Activity Activity { get; }
    protected ToastConfig Config { get; }

    public ToastBuilder(Activity activity, ToastConfig config)
    {
        Activity = activity;
        Config = config;
    }

    public override void OnShown(Snackbar? snackbar)
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
                snackbar!.View.Animate()!.Alpha(0f).SetDuration(FadeInFadeOutAnimationDuration).Start();
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

        snackbar!.View.Animate()!.Alpha(1f).SetDuration(FadeInFadeOutAnimationDuration).Start();
    }

    public override void OnDismissed(Snackbar? snackbar, int e)
    {
        base.OnDismissed(snackbar, e);

        _dismissed?.Invoke();
    }

    public virtual Snackbar Build()
    {
        var snackbar = Snackbar.Make(
            Activity,
            Activity.Window!.DecorView!.RootView!,
            Config.Message,
            (int)Config.Duration.TotalMilliseconds
        );

        SetupSnackbarText(snackbar);

        if (Config.MessageColor is not null)
        {
            snackbar.SetTextColor(Config.MessageColor.ToInt());
        }

        if (Config.BackgroundColor is not null)
        {
            snackbar.View.Background = GetDialogBackground();
        }

        if (snackbar.View.LayoutParameters is FrameLayout.LayoutParams layoutParams)
        {
            layoutParams.Height = FrameLayout.LayoutParams.WrapContent;
            layoutParams.Width = FrameLayout.LayoutParams.WrapContent;
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
        backgroundDrawable.SetColor(Config.BackgroundColor!.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(Config.CornerRadius));

        return backgroundDrawable;
    }

    protected virtual void SetupSnackbarText(Snackbar snackbar)
    {
        var l = ((snackbar.View as Snackbar.SnackbarLayout)!.GetChildAt(0) as SnackbarContentLayout)!;
        var text = (l.GetChildAt(0) as TextView)!;
        text.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)Config.MessageFontSize);

        if (Config.MessageFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Activity.Assets, Config.MessageFontFamily);
            text.SetTypeface(typeface, TypefaceStyle.Normal);
        }

        text.SetMaxLines(int.MaxValue);

        if (Config.Icon is null) return;

        var icon = GetIcon();

        if (IsRTL()) text.SetCompoundDrawables(null, null, icon, null);
        else text.SetCompoundDrawables(icon, null, null, null);
        text.CompoundDrawablePadding = DpToPixels(IconPadding);
    }

    protected virtual Drawable GetIcon()
    {
        var imgId = MauiApplication.Current.GetDrawableId(Config.Icon!);
        var img = MauiApplication.Current.GetDrawable(imgId)!;
        img.ScaleTo(IconSize);

        return img;
    }
}