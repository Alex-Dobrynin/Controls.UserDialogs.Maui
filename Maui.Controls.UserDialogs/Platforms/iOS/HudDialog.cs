using BigTed;

using CoreAnimation;

using CoreGraphics;

using Foundation;

using Microsoft.Maui.Platform;

using UIKit;

namespace Maui.Controls.UserDialogs;

public class HudDialog : IHudDialog
{
    private HudDialogConfig _config;
    private UIButton _cnclBtn;

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

        if (_cnclBtn is not null)
        {
            UIFont font;
            if (_config.FontFamily is not null)
            {
                font = UIFont.FromName(_config.FontFamily, (float)HudDialogConfig.NegativeButtonFontSize);
            }
            else font = UIFont.SystemFontOfSize((float)HudDialogConfig.NegativeButtonFontSize);

            _cnclBtn.SetAttributedTitle(new NSMutableAttributedString(_config.CancelText, font, HudDialogConfig.NegativeButtonTextColor?.ToPlatform()), UIControlState.Normal);
        }
    }

    public void Show()
    {
        if (_config.Image is not null)
        {
            ShowImage();
            return;
        }

        UIApplication.SharedApplication.InvokeOnMainThread(() =>
        {
            var hud = ProgressHUD.For(Extensions.GetDefaultWindow());

            BeforeShow(hud);
            var percent = _config.PercentComplete / 100f;
            if (_config.OnCancel is not null)
            {
                hud.Show(
                    _config.CancelText,
                    _config.OnCancel,
                    _config.Message,
                    percent,
                    _config.MaskType.ToNative()
                    );
            }
            else
            {
                hud.Show(
                    _config.Message,
                    percent,
                    _config.MaskType.ToNative()
                    );
            }

            AfterShow(hud);
        });
    }

    private void ShowImage()
    {
        UIApplication.SharedApplication.InvokeOnMainThread(() =>
        {
            var hud = ProgressHUD.For(Extensions.GetDefaultWindow());

            BeforeShowImage(hud);

            hud.ShowImage(
                new UIImage(_config.Image).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal).ScaleTo(100),
                _config.Message,
                _config.MaskType.ToNative(),
                TimeSpan.MaxValue.TotalMilliseconds
                );

            AfterShowImage(hud);
        });
    }

    private void BeforeShowImage(ProgressHUD hud)
    {
        if (HudDialogConfig.MessageColor is not null)
        {
            hud.HudForegroundColor = HudDialogConfig.MessageColor.ToPlatform();
        }

        UIFont font;
        if (_config.FontFamily is not null)
        {
            font = UIFont.FromName(_config.FontFamily, (float)HudDialogConfig.MessageFontSize);
        }
        else font = UIFont.SystemFontOfSize((float)HudDialogConfig.MessageFontSize);

        hud.HudFont = font;
    }

    private void AfterShowImage(ProgressHUD hud)
    {
        var toolbar = hud.Subviews[0];
        toolbar.Layer.CornerRadius = HudDialogConfig.CornerRadius;
        if (HudDialogConfig.BackgroundColor is not null)
        {
            toolbar.BackgroundColor = HudDialogConfig.BackgroundColor.ToPlatform();
        }

        var bgView = toolbar.Subviews[0];
        bgView.Alpha = 0;

        bool isAlpha0 = bgView.Alpha == 0;
        if(isAlpha0)
        {

        }

        var image = toolbar.Subviews[1] as UIImageView;
        image.Image = image.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
        image.Transform = CGAffineTransform.MakeScale(1.2f, 1.2f);

        UIFont font;
        if (_config.FontFamily is not null)
        {
            font = UIFont.FromName(_config.FontFamily, (float)HudDialogConfig.NegativeButtonFontSize);
        }
        else font = UIFont.SystemFontOfSize((float)HudDialogConfig.NegativeButtonFontSize);

        if (_config.OnCancel is null) return;
        if (toolbar.Subviews[3] is not UIButton button) return;

        _cnclBtn = button;
        _cnclBtn.SetAttributedTitle(new NSMutableAttributedString(_config.CancelText, font, HudDialogConfig.NegativeButtonTextColor?.ToPlatform()), UIControlState.Normal);
    }

    private void BeforeShow(ProgressHUD hud)
    {
        if (HudDialogConfig.MessageColor is not null)
        {
            hud.HudForegroundColor = HudDialogConfig.MessageColor.ToPlatform();
        }
        hud.RingThickness = 4f;
        hud.RingRadius = 22f;

        if (hud.Subviews.FirstOrDefault() is UIToolbar toolbar && HudDialogConfig.ProgressColor is not null)
        {
            var layers = toolbar.Layer.Sublayers.OfType<CAShapeLayer>().ToList();
            if (layers.Count > 0)
            {
                var rimLayer = layers[0];
                rimLayer.StrokeColor = HudDialogConfig.ProgressColor.WithAlpha(0.3f).ToPlatform().CGColor;

                var barLayer = layers[1];
                barLayer.StrokeColor = HudDialogConfig.ProgressColor.ToPlatform().CGColor;
            }
        }

        UIFont font;
        if (_config.FontFamily is not null)
        {
            font = UIFont.FromName(_config.FontFamily, (float)HudDialogConfig.MessageFontSize);
        }
        else font = UIFont.SystemFontOfSize((float)HudDialogConfig.MessageFontSize);

        hud.HudFont = font;
    }

    private void AfterShow(ProgressHUD hud)
    {
        var toolbar = hud.Subviews[0];
        toolbar.Layer.CornerRadius = HudDialogConfig.CornerRadius;
        if (HudDialogConfig.BackgroundColor is not null)
        {
            toolbar.BackgroundColor = HudDialogConfig.BackgroundColor.ToPlatform();
        }

        var bgView = toolbar.Subviews[0];
        bgView.Alpha = 0;

        var indicator = toolbar.Subviews.Last() as UIActivityIndicatorView;
        if (HudDialogConfig.LoaderColor is not null)
        {
            indicator.Color = HudDialogConfig.LoaderColor.ToPlatform();
        }
        indicator.Transform = CGAffineTransform.MakeScale(1.3f, 1.3f);

        UIFont font;
        if (_config.FontFamily is not null)
        {
            font = UIFont.FromName(_config.FontFamily, (float)HudDialogConfig.NegativeButtonFontSize);
        }
        else font = UIFont.SystemFontOfSize((float)HudDialogConfig.NegativeButtonFontSize);

        if (_config.OnCancel is null) return;
        if (toolbar.Subviews[3] is not UIButton button) return;

        _cnclBtn = button;
        _cnclBtn.SetAttributedTitle(new NSMutableAttributedString(_config.CancelText, font, HudDialogConfig.NegativeButtonTextColor?.ToPlatform()), UIControlState.Normal);
    }

    public void Hide()
    {
        try
        {
            _cnclBtn = null;
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var hud = ProgressHUD.For(Extensions.GetDefaultWindow());
                hud.Dismiss();
            });
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