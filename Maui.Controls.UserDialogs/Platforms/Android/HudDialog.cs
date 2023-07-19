using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;

using AndroidHUD;

using Microsoft.Maui.Platform;

using static Maui.Controls.UserDialogs.Extensions;

using Platform = Microsoft.Maui.ApplicationModel.Platform;

namespace Maui.Controls.UserDialogs;

public class HudDialog : IHudDialog
{
    private HudDialogConfig _config;
    private Android.Widget.Button _cnclBtn;
    private TextView _progressText;

    public void Update(string message = null, int percentComplete = -1, string image = null, string cancelText = null, bool show = true, MaskType? maskType = null, Action onCancel = null)
    {
        this.Update(new HudDialogConfig()
        {
            Message = message,
            CancelText = cancelText ?? HudDialogConfig.DefaultCancelText,
            PercentComplete = percentComplete,
            Image = image,
            AutoShow = show,
            MaskType = maskType ?? HudDialogConfig.DefaultMaskType,
            OnCancel = onCancel
        });
    }

    public void Update(HudDialogConfig config)
    {
        _config = config;
        if (_config.AutoShow) Show();

        if (config.PercentComplete == -1) _progressText = null;

        if (_progressText is not null)
        {
            _progressText.Text = $"{config.PercentComplete}%";
        }
        if (_cnclBtn is not null)
        {
            _cnclBtn.Clickable = config.OnCancel is not null;
            _cnclBtn.Visibility = config.OnCancel is null ? ViewStates.Gone : ViewStates.Visible;
        }
    }

    public void Show()
    {
        if (_config.Image is not null)
        {
            ShowImage();
            return;
        }

        AndHUD.Shared.Show(
            Platform.CurrentActivity,
            _config.Message,
            _config.PercentComplete,
            _config.MaskType.ToNative(),
            null,
            null,
            true,
            null,
            BeforeShow,
            null
        );
    }

    private void ShowImage()
    {
        var imgId = MauiApplication.Current.GetDrawableId(_config.Image);

        AndHUD.Shared.ShowImage(
            Platform.CurrentActivity,
            imgId,
            _config.Message,
            _config.MaskType.ToNative(),
            null,
            null,
            null,
            BeforeShow,
            null
        );
    }

    protected virtual void BeforeShow(Dialog dialog)
    {
        if (dialog is null)
            return;

        Typeface typeFace = null;
        if (_config.FontFamily is not null)
        {
            typeFace = Typeface.CreateFromAsset(Platform.CurrentActivity.Assets, _config.FontFamily);
        }

        dialog.Window.AddFlags(WindowManagerFlags.NotFocusable);

        var textViewId = Platform.CurrentActivity.Resources.GetIdentifier("textViewStatus", "id", Platform.CurrentActivity.PackageName);
        var textView = dialog.FindViewById<TextView>(textViewId);

        textView.SetTextColor(HudDialogConfig.MessageColor.ToPlatform());
        textView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)HudDialogConfig.MessageFontSize);
        textView.SetTypeface(typeFace, TypefaceStyle.Normal);

        var parent = textView.Parent as RelativeLayout;

        parent.Background = GetDialogBackground(_config);

        var spinnerId = Platform.CurrentActivity.Resources.GetIdentifier("loadingProgressBar", "id", Platform.CurrentActivity.PackageName);
        var progressBar = dialog.FindViewById<Android.Widget.ProgressBar>(spinnerId);

        if (progressBar is not null)
        {
            progressBar.LayoutParameters.Width = DpToPixels(50);
            progressBar.LayoutParameters.Height = DpToPixels(50);

            var spinnerDraw = progressBar.IndeterminateDrawable;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                spinnerDraw.SetColorFilter(new BlendModeColorFilter(HudDialogConfig.LoaderColor.ToPlatform(), Android.Graphics.BlendMode.SrcIn));
            }
            else
            {
                spinnerDraw.SetColorFilter(HudDialogConfig.LoaderColor.ToPlatform(), PorterDuff.Mode.SrcIn);
            }
        }

        var progressId = Platform.CurrentActivity.Resources.GetIdentifier("loadingProgressWheel", "id", Platform.CurrentActivity.PackageName);
        var progressWheel = dialog.FindViewById<ProgressWheel>(progressId);
        if (progressWheel is not null)
        {
            progressWheel.BarColor = HudDialogConfig.ProgressColor.ToPlatform();
            progressWheel.RimColor = HudDialogConfig.ProgressColor.WithAlpha(0.3f).ToPlatform();
            var rParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            rParams.AddRule(LayoutRules.AlignBottom, progressWheel.Id);
            rParams.AddRule(LayoutRules.AlignTop, progressWheel.Id);
            rParams.AddRule(LayoutRules.AlignLeft, progressWheel.Id);
            rParams.AddRule(LayoutRules.AlignRight, progressWheel.Id);
            rParams.AddRule(LayoutRules.CenterHorizontal);

            _progressText = new TextView(Platform.CurrentActivity)
            {
                Text = $"{_config.PercentComplete}%",
                TextAlignment = Android.Views.TextAlignment.Gravity,
                Gravity = GravityFlags.Center,
                LayoutParameters = rParams,
            };

            _progressText.SetTextColor(HudDialogConfig.ProgressColor.ToPlatform());
            _progressText.SetTextSize(Android.Util.ComplexUnitType.Sp, 14f);
            _progressText.SetTypeface(typeFace, TypefaceStyle.Normal);

            parent.AddView(_progressText);
        }

        if (_config.OnCancel is not null)
        {
            int id = textViewId;

            var rParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            rParams.AddRule(LayoutRules.Below, id);
            rParams.AddRule(LayoutRules.CenterHorizontal);

            _cnclBtn = new Android.Widget.Button(Platform.CurrentActivity)
            {
                Text = _config.CancelText,
                TextAlignment = Android.Views.TextAlignment.Gravity,
                Gravity = GravityFlags.Center,
                LayoutParameters = rParams
            };

            _cnclBtn.SetBackgroundColor(Colors.Transparent.ToPlatform());
            _cnclBtn.SetTextColor(HudDialogConfig.NegativeButtonTextColor.ToPlatform());
            _cnclBtn.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)HudDialogConfig.NegativeButtonFontSize);
            _cnclBtn.SetTypeface(typeFace, TypefaceStyle.Normal);
            _cnclBtn.Click += (s, e) =>
            {
                OnCancelClick();
            };

            parent.AddView(_cnclBtn);
        }
    }

    protected virtual Drawable GetDialogBackground(HudDialogConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(HudDialogConfig.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(HudDialogConfig.CornerRadius));

        return backgroundDrawable;
    }

    void OnCancelClick()
    {
        if (_config.OnCancel is null)
            return;

        Hide();
        _config.OnCancel();
    }

    public void Hide()
    {
        try
        {
            _progressText = null;
            _cnclBtn = null;
            AndHUD.Shared.Dismiss(Platform.CurrentActivity);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception ({ex.GetType().FullName}) occured while dismissing dialog: {ex.Message}");
        }
    }

    public void Dispose()
    {
        Hide();
        GC.SuppressFinalize(this);
    }
}