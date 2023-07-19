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
            Cancel = onCancel
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
                font = UIFont.FromName(_config.FontFamily, (float)config.NegativeButtonFontSize);
            }
            else font = UIFont.SystemFontOfSize((float)config.NegativeButtonFontSize);

            _cnclBtn.SetAttributedTitle(new NSMutableAttributedString(_config.CancelText, font, config.NegativeButtonTextColor?.ToPlatform()), UIControlState.Normal);
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
            if (_config.Cancel is not null)
            {
                hud.Show(
                    _config.CancelText,
                    _config.Cancel,
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
        if (_config.MessageColor is not null)
        {
            hud.HudForegroundColor = _config.MessageColor.ToPlatform();
        }

        UIFont font;
        if (_config.FontFamily is not null)
        {
            font = UIFont.FromName(_config.FontFamily, (float)_config.MessageFontSize);
        }
        else font = UIFont.SystemFontOfSize((float)_config.MessageFontSize);

        hud.HudFont = font;
    }

    private void AfterShowImage(ProgressHUD hud)
    {
        var toolbar = hud.Subviews[0];
        toolbar.Layer.CornerRadius = _config.CornerRadius;
        if (_config.BackgroundColor is not null)
        {
            toolbar.BackgroundColor = _config.BackgroundColor.ToPlatform();
        }

        var bgView = toolbar.Subviews[0];
        bgView.Alpha = 0;

        bool isAlpha0 = bgView.Alpha == 0;
        if (isAlpha0)
        {

        }

        var image = toolbar.Subviews[1] as UIImageView;
        image.Image = image.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
        image.Transform = CGAffineTransform.MakeScale(1.2f, 1.2f);

        UIFont font;
        if (_config.FontFamily is not null)
        {
            font = UIFont.FromName(_config.FontFamily, (float)_config.NegativeButtonFontSize);
        }
        else font = UIFont.SystemFontOfSize((float)_config.NegativeButtonFontSize);

        if (_config.Cancel is null) return;
        if (toolbar.Subviews[3] is not UIButton button) return;

        _cnclBtn = button;
        _cnclBtn.SetAttributedTitle(new NSMutableAttributedString(_config.CancelText, font, _config.NegativeButtonTextColor?.ToPlatform()), UIControlState.Normal);
    }

    private void BeforeShow(ProgressHUD hud)
    {
        if (_config.MessageColor is not null)
        {
            hud.HudForegroundColor = _config.MessageColor.ToPlatform();
        }
        hud.RingThickness = 4f;
        hud.RingRadius = 22f;

        if (hud.Subviews.FirstOrDefault() is UIToolbar toolbar && _config.ProgressColor is not null)
        {
            var layers = toolbar.Layer.Sublayers.OfType<CAShapeLayer>().ToList();
            if (layers.Count > 0)
            {
                var rimLayer = layers[0];
                rimLayer.StrokeColor = _config.ProgressColor.WithAlpha(0.3f).ToPlatform().CGColor;

                var barLayer = layers[1];
                barLayer.StrokeColor = _config.ProgressColor.ToPlatform().CGColor;
            }
        }

        UIFont font;
        if (_config.FontFamily is not null)
        {
            font = UIFont.FromName(_config.FontFamily, (float)_config.MessageFontSize);
        }
        else font = UIFont.SystemFontOfSize((float)_config.MessageFontSize);

        hud.HudFont = font;
    }

    private void AfterShow(ProgressHUD hud)
    {
        var toolbar = hud.Subviews[0];
        toolbar.Layer.CornerRadius = _config.CornerRadius;
        if (_config.BackgroundColor is not null)
        {
            toolbar.BackgroundColor = _config.BackgroundColor.ToPlatform();
        }

        var bgView = toolbar.Subviews[0];
        bgView.Alpha = 0;

        var indicator = toolbar.Subviews.OfType<UIActivityIndicatorView>().FirstOrDefault();
        if (_config.LoaderColor is not null)
        {
            indicator.Color = _config.LoaderColor.ToPlatform();
        }
        indicator.Transform = CGAffineTransform.MakeScale(1.3f, 1.3f);

        UIFont font;
        if (_config.FontFamily is not null)
        {
            font = UIFont.FromName(_config.FontFamily, (float)_config.NegativeButtonFontSize);
        }
        else font = UIFont.SystemFontOfSize((float)_config.NegativeButtonFontSize);

        if (_config.Cancel is null) return;
        var button = toolbar.Subviews.OfType<UIButton>().FirstOrDefault();
        if (button is null) return;

        _cnclBtn = button;
        _cnclBtn.SetAttributedTitle(new NSMutableAttributedString(_config.CancelText, font, _config.NegativeButtonTextColor?.ToPlatform()), UIControlState.Normal);
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