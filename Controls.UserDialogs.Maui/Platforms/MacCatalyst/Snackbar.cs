﻿using CoreGraphics;

using Foundation;

using Microsoft.Maui.Platform;

using UIKit;

namespace Controls.UserDialogs.Maui;

public enum Position
{
    Bottom,
    Top
}

public enum Style
{
    Toast,
    Snackbar
}

public class Snackbar : UIView
{
    private System.Timers.Timer _timer;
    private DateTime _endOfAnimation;

    public static double DefaultIconSize { get; set; } = 26;
    public static double DefaultActionIconSize { get; set; } = 24;
    public static Thickness DefaultPadding { get; set; } = new Thickness(20);
    public static Thickness DefaultToastMargin { get; set; } = new Thickness(20, 50, 20, 80);
    public static Thickness DefaultSnackbarMargin { get; set; } = new Thickness(20, 50, 20, 30);
    public static bool DefaultUseBlur { get; set; } = true;
    public static bool DefaultUseAnimation { get; set; } = true;
    public static TimeSpan DefaultAnimationDuration { get; set; } = TimeSpan.FromMilliseconds(250);
    public static TimeSpan DefaultDismissDuration { get; set; } = TimeSpan.FromSeconds(3);
    public static float DefaultCornerRadius { get; set; } = 15f;
    public static float DefaultIconSpacing { get; set; } = 15f;
    public static UIBlurEffectStyle DefaultBlurEffectStyle { get; set; } = UIBlurEffectStyle.Dark;
    public static UIColor DefaultBackgroundColor { get; set; } = Colors.Black.WithAlpha(0.15f).ToPlatform();

    public double IconSize { get; set; } = DefaultIconSize;
    public double ActionIconSize { get; set; } = DefaultActionIconSize;
    public Thickness Padding { get; set; } = DefaultPadding;
    public Thickness ToastMargin { get; set; } = DefaultToastMargin;
    public Thickness SnackbarMargin { get; set; } = DefaultSnackbarMargin;
    public float MessageFontSize { get; set; } = 16f;
    public float ActionFontSize { get; set; } = 20f;
    public string Message { get; set; }
    public string Icon { get; set; }
    public string FontFamily { get; set; }
    public string CancelButtonFontFamily { get; set; }
    public UIColor MessageColor { get; set; } = Colors.White.ToPlatform();
    public UIColor ActionColor { get; set; } = Colors.White.ToPlatform();
    public Position Position { get; set; }
    public Style Style { get; set; }
    public string ActionText { get; set; }
    public string ActionIcon { get; set; }
    public Action Action { get; set; }
    public bool UseBlur { get; set; } = DefaultUseBlur;
    public bool UseAnimation { get; set; } = DefaultUseAnimation;
    public TimeSpan AnimationDuration { get; set; } = DefaultAnimationDuration;
    public TimeSpan DismissDuration { get; set; } = DefaultDismissDuration;
    public float CornerRadius
    {
        get => (float)this.Layer.CornerRadius;
        set => this.Layer.CornerRadius = value;
    }
    public float IconSpacing { get; set; } = DefaultIconSpacing;
    public UIBlurEffectStyle BlurEffectStyle { get; set; } = DefaultBlurEffectStyle;
    public bool ShowCountDown { get; set; }
    public event EventHandler Timeout;

    public Snackbar()
    {
        this.TranslatesAutoresizingMaskIntoConstraints = false;
        BackgroundColor = DefaultBackgroundColor;
        CornerRadius = DefaultCornerRadius;
    }

    public virtual void Dismiss()
    {
        try
        {
            _timer?.Stop();
        }
        catch { }
        try
        {
            this.RemoveFromSuperview();
        }
        catch { }
    }

    public virtual void Show()
    {
        this.ClearSubviews();

        if (UseBlur) SetupBlur();

        var window = Extensions.GetDefaultWindow();
        window.AddSubview(this);

        var constraints = new List<NSLayoutConstraint>
        {
            this.CenterXAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.CenterXAnchor)
        };

        _timer = new System.Timers.Timer
        {
            Interval = 500
        };

        var popup = Style is Style.Toast ? SetupToast() : SetupSnackBar();

        this.AddSubview(popup);

        NSLayoutConstraint.ActivateConstraints(
        [
            popup.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor, (float)Padding.Left),
            popup.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor, (float)-Padding.Right),
            popup.TopAnchor.ConstraintEqualTo(this.TopAnchor, (float)Padding.Top),
            popup.BottomAnchor.ConstraintEqualTo(this.BottomAnchor, (float)-Padding.Bottom),
        ]);

        if (Style is Style.Toast)
        {
            constraints.Add(this.LeadingAnchor.ConstraintGreaterThanOrEqualTo(window.SafeAreaLayoutGuide.LeadingAnchor, (float)ToastMargin.Left));
            constraints.Add(this.TrailingAnchor.ConstraintLessThanOrEqualTo(window.SafeAreaLayoutGuide.TrailingAnchor, (float)ToastMargin.Right));

            constraints.Add(Position is Position.Bottom
                ? this.BottomAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.BottomAnchor, (float)-ToastMargin.Bottom)
                : this.TopAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.TopAnchor, (float)ToastMargin.Top));
        }
        else
        {
            constraints.Add(this.LeadingAnchor.ConstraintGreaterThanOrEqualTo(window.SafeAreaLayoutGuide.LeadingAnchor, (float)SnackbarMargin.Left));
            constraints.Add(this.TrailingAnchor.ConstraintLessThanOrEqualTo(window.SafeAreaLayoutGuide.TrailingAnchor, (float)SnackbarMargin.Right));
            constraints.Add(this.WidthAnchor.ConstraintGreaterThanOrEqualTo(600));

            constraints.Add(Position is Position.Bottom
                ? this.BottomAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.BottomAnchor, (float)-SnackbarMargin.Bottom)
                : this.TopAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.TopAnchor, (float)SnackbarMargin.Top));
        }

        NSLayoutConstraint.ActivateConstraints(constraints.ToArray());

        if (UseAnimation)
        {
            this.Alpha = 0;

            UIView.Animate(AnimationDuration.TotalSeconds, () =>
            {
                this.Alpha = 1f;
            });
        }

        _endOfAnimation = DateTime.Now + DismissDuration;
        _timer.Elapsed += (s, a) =>
        {
            var rest = (_endOfAnimation - a.SignalTime).TotalSeconds;
            if (rest > 0) return;
            _timer.Stop();

            if (!UseAnimation) return;

            this.InvokeOnMainThread(() =>
            {
                UIView.Animate(AnimationDuration.TotalSeconds,
                    () =>
                    {
                        this.Alpha = 0f;
                    },
                    () =>
                    {
                        this.Dismiss();
                        this.Timeout?.Invoke(this, EventArgs.Empty);
                    });
            });
        };
        _timer.Start();
    }

    protected virtual void SetupBlur()
    {
        var blurEffect = UIBlurEffect.FromStyle(BlurEffectStyle);
        var effectsView = new UIVisualEffectView
        {
            Effect = blurEffect,
            TranslatesAutoresizingMaskIntoConstraints = false,
            ClipsToBounds = true
        };
        effectsView.Layer.CornerRadius = CornerRadius;

        this.AddSubview(effectsView);

        NSLayoutConstraint.ActivateConstraints(
        [
            effectsView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor),
            effectsView.TrailingAnchor.ConstraintEqualTo(this.TrailingAnchor),
            effectsView.TopAnchor.ConstraintEqualTo(this.TopAnchor),
            effectsView.BottomAnchor.ConstraintEqualTo(this.BottomAnchor),
        ]);
    }

    protected virtual UIView SetupToast()
    {
        var container = new UIStackView
        {
            Spacing = IconSpacing,
            Alignment = UIStackViewAlignment.Center,
            Axis = UILayoutConstraintAxis.Horizontal,
            TranslatesAutoresizingMaskIntoConstraints = false
        };

        if (Icon is not null)
        {
            container.AddArrangedSubview(GetIcon());
        }

        container.AddArrangedSubview(GetLabel());

        return container;
    }

    protected virtual UIView SetupSnackBar()
    {
        var toast = SetupToast();

        var container = new UIStackView
        {
            Spacing = IconSpacing,
            Alignment = UIStackViewAlignment.Center,
            Axis = UILayoutConstraintAxis.Horizontal,
            TranslatesAutoresizingMaskIntoConstraints = false
        };
        container.AddArrangedSubview(toast);

        var action = GetAction();

        var widthConstraint = NSLayoutConstraint.Create(action, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1f, action.IntrinsicContentSize.Width);
        widthConstraint.Priority = (int)UILayoutPriority.Required;
        action.AddConstraint(widthConstraint);

        container.AddArrangedSubview(action);

        return container;
    }

    protected virtual UIView GetAction()
    {
        var container = new UIStackView
        {
            Spacing = IconSpacing,
            Alignment = UIStackViewAlignment.Center,
            Axis = UILayoutConstraintAxis.Horizontal,
            TranslatesAutoresizingMaskIntoConstraints = false
        };

        container.AddArrangedSubview(GetActionButton());
        if (ShowCountDown)
        {
            container.AddArrangedSubview(GetCountDown());
        }

        return container;
    }

    protected virtual UIView GetActionButton()
    {
        var button = new UIButton
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
        };
        button.TouchUpInside += (s, a) =>
        {
            this.Dismiss();
            Action?.Invoke();
        };

        UIFont font = null;
        if (CancelButtonFontFamily is not null)
        {
            font = UIFont.FromName(CancelButtonFontFamily, ActionFontSize);
        }
        font ??= UIFont.SystemFontOfSize(ActionFontSize, UIFontWeight.Bold);

        button.SetAttributedTitle(new NSMutableAttributedString(ActionText, font, ActionColor), UIControlState.Normal);

        if (OperatingSystem.IsMacCatalystVersionAtLeast(15) || OperatingSystem.IsIOSVersionAtLeast(15))
        {
            var configuration = UIButtonConfiguration.PlainButtonConfiguration;
            configuration.ImagePadding = 10;
            configuration.ContentInsets = new NSDirectionalEdgeInsets(0, 0, 0, 0);
            button.Configuration = configuration;
        }
        else
        {
            button.ImageEdgeInsets = new UIEdgeInsets(0, 0, 0, 10f);
        }

        if (ActionIcon is not null)
        {
            button.SetImage(UIImage.FromBundle(ActionIcon).ScaleTo(ActionIconSize), UIControlState.Normal);
        }

        var widthConstraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1f, button.IntrinsicContentSize.Width);
        widthConstraint.Priority = (int)UILayoutPriority.Required;
        button.AddConstraint(widthConstraint);

        return button;
    }

    protected virtual UIView GetCountDown()
    {
        UIFont font = null;
        if (FontFamily is not null)
        {
            font = UIFont.FromName(FontFamily, MessageFontSize);
        }
        font ??= UIFont.SystemFontOfSize(MessageFontSize);

        var labelCounter = new UILabel
        {
            TextColor = ActionColor,
            TranslatesAutoresizingMaskIntoConstraints = false,
            Text = "" + (int)DismissDuration.TotalSeconds,
            Font = font
        };

        _timer.Elapsed += (s, a) =>
        {
            var rest = (_endOfAnimation - a.SignalTime).TotalSeconds;
            if (rest <= 0)
            {
                _timer.Stop();
                return;
            }
            this.InvokeOnMainThread(() => { labelCounter.Text = "" + Math.Round(rest); });
        };

        var widthConstraint = NSLayoutConstraint.Create(labelCounter, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1f, labelCounter.IntrinsicContentSize.Width);
        widthConstraint.Priority = (int)UILayoutPriority.Required;
        labelCounter.AddConstraint(widthConstraint);

        return labelCounter;
    }

    protected virtual UIView GetIcon()
    {
        var image = new UIImageView(new CGRect(0, 0, IconSize, IconSize))
        {
            Image = UIImage.FromBundle(Icon).ScaleTo(IconSize),
            ContentMode = UIViewContentMode.Center,
            TranslatesAutoresizingMaskIntoConstraints = false
        };
        return image;
    }

    protected virtual UIView GetLabel()
    {
        UIFont font = null;
        if (FontFamily is not null)
        {
            font = UIFont.FromName(FontFamily, MessageFontSize);
        }
        font ??= UIFont.SystemFontOfSize(MessageFontSize);

        var label = new UILabel
        {
            Text = Message,
            TextColor = MessageColor,
            Font = font,
            LineBreakMode = UILineBreakMode.WordWrap,
            Lines = 0,
            TranslatesAutoresizingMaskIntoConstraints = false
        };

        return label;
    }
}