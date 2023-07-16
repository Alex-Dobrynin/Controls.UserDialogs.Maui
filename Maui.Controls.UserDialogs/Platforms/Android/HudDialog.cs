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
    private Action<int> _onProgressChange;
    private Action<string> _onMessageChange;
    private Action<Action> _onCancelChange;

    public void Update(HudDialogConfig config)
    {
        _config = config;
        if (_config.AutoShow) Show();

        if (config.PercentComplete == -1) _onProgressChange = null;

        _onProgressChange?.Invoke(config.PercentComplete);
        _onMessageChange?.Invoke(config.Message);
        _onCancelChange?.Invoke(config.OnCancel);
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
            _config.Title,
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
            _config.Title,
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

        textView.SetTextColor(HudDialogConfig.TitleColor.ToPlatform());
        textView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)HudDialogConfig.TitleFontSize);
        textView.SetTypeface(typeFace, TypefaceStyle.Normal);

        var parent = textView.Parent as RelativeLayout;

        parent.Background = GetDialogBackground(_config);
        int messageViewId = Android.Views.View.GenerateViewId();
        if (_config.Message is not null)
        {
            var rParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            rParams.AddRule(LayoutRules.Below, textView.Id);
            rParams.AddRule(LayoutRules.CenterHorizontal);

            var messageView = new TextView(Platform.CurrentActivity)
            {
                Text = _config.Message,
                TextAlignment = Android.Views.TextAlignment.Gravity,
                Gravity = GravityFlags.Center,
                LayoutParameters = rParams,
                Id = messageViewId
            };
            messageView.SetTextColor(HudDialogConfig.MessageColor.ToPlatform());
            messageView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)HudDialogConfig.MessageFontSize);
            messageView.SetTypeface(typeFace, TypefaceStyle.Normal);

            _onMessageChange = m =>
            {
                messageView.Visibility = m is null ? ViewStates.Gone : ViewStates.Visible;

                messageView.Text = m;
            };

            parent.AddView(messageView);
        }

        var spinnerId = Platform.CurrentActivity.Resources.GetIdentifier("loadingProgressBar", "id", Platform.CurrentActivity.PackageName);
        var progressBar = dialog.FindViewById<Android.Widget.ProgressBar>(spinnerId);

        if (progressBar is not null)
        {
            progressBar.LayoutParameters.Width = DpToPixels(60);
            progressBar.LayoutParameters.Height = DpToPixels(60);

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

            var progressText = new TextView(Platform.CurrentActivity)
            {
                Text = $"{_config.PercentComplete}%",
                TextAlignment = Android.Views.TextAlignment.Gravity,
                Gravity = GravityFlags.Center,
                LayoutParameters = rParams,
            };

            progressText.SetTextColor(HudDialogConfig.ProgressColor.ToPlatform());
            progressText.SetTextSize(Android.Util.ComplexUnitType.Sp, 14f);
            progressText.SetTypeface(typeFace, TypefaceStyle.Normal);

            _onProgressChange = p =>
            {
                progressText.Text = $"{p}%";
            };

            parent.AddView(progressText);
        }

        if (_config.OnCancel is not null)
        {
            int id = _config.Message is not null ? messageViewId : textViewId;

            var rParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            rParams.AddRule(LayoutRules.Below, id);
            rParams.AddRule(LayoutRules.CenterHorizontal);

            var cnclBtn = new Android.Widget.Button(Platform.CurrentActivity)
            {
                Text = _config.CancelText,
                TextAlignment = Android.Views.TextAlignment.Gravity,
                Gravity = GravityFlags.Center,
                LayoutParameters = rParams
            };

            cnclBtn.SetBackgroundColor(Colors.Transparent.ToPlatform());
            cnclBtn.SetTextColor(HudDialogConfig.NegativeButtonTextColor.ToPlatform());
            cnclBtn.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)HudDialogConfig.NegativeButtonFontSize);
            cnclBtn.SetTypeface(typeFace, TypefaceStyle.Normal);
            cnclBtn.Click += (s, e) =>
            {
                OnCancelClick();
            };

            _onCancelChange = a =>
            {
                cnclBtn.Clickable = a is not null;
                cnclBtn.Visibility = a is null ? ViewStates.Gone : ViewStates.Visible;
            };

            parent.AddView(cnclBtn);
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
            _onProgressChange = null;
            _onMessageChange = null;
            _onCancelChange = null;
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