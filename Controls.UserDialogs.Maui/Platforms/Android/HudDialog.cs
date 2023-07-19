using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;

using AndroidHUD;

using Microsoft.Maui.Platform;

using static Controls.UserDialogs.Maui.Extensions;

using Platform = Microsoft.Maui.ApplicationModel.Platform;

namespace Controls.UserDialogs.Maui;

public class HudDialog : IHudDialog
{
    public static double DefaultProgressSize { get; set; } = 60;
    public static bool DefaultShowProgresPercents { get; set; } = true;

    public double ProgressSize { get; set; } = DefaultProgressSize;
    public bool ShowProgresPercents { get; set; } = DefaultShowProgresPercents;

    private HudDialogConfig _config;
    private Android.Widget.Button _cnclBtn;
    private TextView _progressText;

    public void Update(string message = null, int percentComplete = -1, string image = null, string cancelText = null, bool show = true, MaskType? maskType = null, Action cancel = null)
    {
        this.Update(new HudDialogConfig()
        {
            Message = message,
            CancelText = cancelText ?? HudDialogConfig.DefaultCancelText,
            PercentComplete = percentComplete,
            Image = image,
            AutoShow = show,
            MaskType = maskType ?? HudDialogConfig.DefaultMaskType,
            Cancel = cancel
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
    }

    public void Show()
    {
        var dialog = AndHUD.Shared.CurrentDialog;

        if (_config.Image is not null)
        {
            ShowImage();
        }
        else
        {
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

        var newDialog = AndHUD.Shared.CurrentDialog;

        if (dialog == newDialog)
        {
            AfterShow(dialog);
        }
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

    protected virtual void AfterShow(Dialog dialog)
    {
        if (dialog is null) return;

        if (_cnclBtn is not null)
        {
            _cnclBtn.Clickable = _config.Cancel is not null;
            _cnclBtn.Visibility = _config.Cancel is null ? ViewStates.Gone : ViewStates.Visible;
        }
        else SetupCancel(dialog);
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

        if (_config.MessageColor is not null)
        {
            textView.SetTextColor(_config.MessageColor.ToPlatform());
        }
        textView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_config.MessageFontSize);
        textView.SetTypeface(typeFace, TypefaceStyle.Normal);

        var parent = textView.Parent as RelativeLayout;

        if (_config.BackgroundColor is not null)
        {
            parent.Background = GetDialogBackground(_config);
        }

        var spinnerId = Platform.CurrentActivity.Resources.GetIdentifier("loadingProgressBar", "id", Platform.CurrentActivity.PackageName);
        var progressBar = dialog.FindViewById<Android.Widget.ProgressBar>(spinnerId);

        if (progressBar is not null)
        {
            progressBar.LayoutParameters.Width = DpToPixels(ProgressSize);
            progressBar.LayoutParameters.Height = DpToPixels(ProgressSize);

            var spinnerDraw = progressBar.IndeterminateDrawable;
            if (_config.LoaderColor is not null)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    spinnerDraw.SetColorFilter(new BlendModeColorFilter(_config.LoaderColor.ToPlatform(), Android.Graphics.BlendMode.SrcIn));
                }
                else
                {
                    spinnerDraw.SetColorFilter(_config.LoaderColor.ToPlatform(), PorterDuff.Mode.SrcIn);
                }
            }
        }

        var progressId = Platform.CurrentActivity.Resources.GetIdentifier("loadingProgressWheel", "id", Platform.CurrentActivity.PackageName);
        var progressWheel = dialog.FindViewById<ProgressWheel>(progressId);
        if (progressWheel is not null)
        {
            progressWheel.LayoutParameters.Width = DpToPixels(ProgressSize);
            progressWheel.LayoutParameters.Height = DpToPixels(ProgressSize);

            if (_config.ProgressColor is not null)
            {
                progressWheel.BarColor = _config.ProgressColor.ToPlatform();
                progressWheel.RimColor = _config.ProgressColor.WithAlpha(0.3f).ToPlatform();
            }

            if (ShowProgresPercents)
            {
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

                if (_config.ProgressColor is not null)
                {
                    _progressText.SetTextColor(_config.ProgressColor.ToPlatform());
                }
                _progressText.SetTextSize(Android.Util.ComplexUnitType.Sp, 14f);
                _progressText.SetTypeface(typeFace, TypefaceStyle.Normal);

                parent.AddView(_progressText);
            }
        }

        SetupCancel(dialog);
    }

    protected virtual void SetupCancel(Dialog dialog)
    {
        if (_config.Cancel is null) return;

        int textViewId = Platform.CurrentActivity.Resources.GetIdentifier("textViewStatus", "id", Platform.CurrentActivity.PackageName);
        var textView = dialog.FindViewById<TextView>(textViewId);
        var parent = textView.Parent as RelativeLayout;

        Typeface typeFace = null;
        if (_config.FontFamily is not null)
        {
            typeFace = Typeface.CreateFromAsset(Platform.CurrentActivity.Assets, _config.FontFamily);
        }

        var rParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
        rParams.AddRule(LayoutRules.Below, textViewId);
        rParams.AddRule(LayoutRules.CenterHorizontal);

        _cnclBtn = new Android.Widget.Button(Platform.CurrentActivity)
        {
            Text = _config.CancelText,
            TextAlignment = Android.Views.TextAlignment.Gravity,
            Gravity = GravityFlags.Center,
            LayoutParameters = rParams
        };

        _cnclBtn.SetBackgroundColor(Colors.Transparent.ToPlatform());
        if (_config.NegativeButtonTextColor is not null)
        {
            _cnclBtn.SetTextColor(_config.NegativeButtonTextColor.ToPlatform());
        }
        _cnclBtn.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_config.NegativeButtonFontSize);
        _cnclBtn.SetTypeface(typeFace, TypefaceStyle.Normal);
        _cnclBtn.Click += (s, e) =>
        {
            OnCancelClick();
        };

        parent.AddView(_cnclBtn);
    }

    protected virtual Drawable GetDialogBackground(HudDialogConfig config)
    {
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetColor(_config.BackgroundColor.ToInt());
        backgroundDrawable.SetCornerRadius(DpToPixels(_config.CornerRadius));

        return backgroundDrawable;
    }

    void OnCancelClick()
    {
        if (_config.Cancel is null)
            return;

        Hide();
        _config.Cancel();
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