using CoreGraphics;

using Microsoft.Maui.Platform;

using UIKit;

namespace Controls.UserDialogs.Maui;

public partial class UserDialogsImplementation
{
    public virtual partial IDisposable Alert(AlertConfig config) => this.Present(() =>
    {
        return new AlertBuilder(config).Build();
    });

    public virtual partial IDisposable Confirm(ConfirmConfig config) => this.Present(() =>
    {
        return new ConfirmBuilder(config).Build();
    });

    public virtual partial IDisposable ActionSheet(ActionSheetConfig config) => this.Present(() =>
    {
        return new ActionSheetBuilder(config).Build();
    });

    public virtual partial IDisposable ShowToast(ToastConfig config)
    {
        Snackbar bar = null;
        var app = UIApplication.SharedApplication;

        app.SafeInvokeOnMainThread(() =>
        {
            bar = new Snackbar()
            {
                Message = config.Message,
                Icon = config.Icon,
                MessageFontSize = (float)config.MessageFontSize,
                CornerRadius = config.CornerRadius,
                DismissDuration = config.Duration,
                FontFamily = config.MessageFontFamily,
                Position = config.Position.ToNative(),
            };
            bar.BackgroundColor = config.BackgroundColor.ToPlatform();
            bar.MessageColor = config.MessageColor.ToPlatform();
            bar.Show();
        });

        return new DisposableAction(() => app.SafeInvokeOnMainThread(() => bar.Dismiss()));
    }

    public virtual partial IDisposable ShowSnackbar(SnackbarConfig config)
    {
        Snackbar bar = null;
        var app = UIApplication.SharedApplication;

        app.SafeInvokeOnMainThread(() =>
        {
            bar = new Snackbar
            {
                Message = config.Message,
                Icon = config.Icon,
                MessageFontSize = (float)config.MessageFontSize,
                CornerRadius = config.CornerRadius,
                DismissDuration = config.Duration,
                FontFamily = config.MessageFontFamily,
                CancelButtonFontFamily = config.NegativeButtonFontFamily,
                Position = config.Position.ToNative(),
                Style = Style.Snackbar,
                ActionText = config.ActionText,
                ActionIcon = config.ActionIcon,
                ShowCountDown = config.ShowCountDown,
                Action = () =>
                {
                    config.Action?.Invoke(SnackbarActionType.UserInteraction);
                }
            };
            bar.BackgroundColor = config.BackgroundColor?.ToPlatform() ?? bar.BackgroundColor;
            bar.MessageColor = config.MessageColor?.ToPlatform() ?? bar.MessageColor;
            bar.ActionColor = config.NegativeButtonTextColor?.ToPlatform() ?? bar.ActionColor;
            bar.Show();
            bar.Timeout += (s, a) =>
            {
                config.Action?.Invoke(SnackbarActionType.Timeout);
            };
        });

        return new DisposableAction(() => app.SafeInvokeOnMainThread(() =>
        {
            bar.Dismiss();
            config.Action?.Invoke(SnackbarActionType.Cancelled);
        }));
    }

    protected virtual partial IHudDialog CreateHudInstance(HudDialogConfig config)
    {
        CurrentHudDialog?.Dispose();

        var dialog = new HudDialog();
        dialog.Update(config);

        return dialog;
    }

    protected virtual IDisposable Present(Func<UIAlertController> alertFunc)
    {
        UIAlertController alert = null;
        var app = UIApplication.SharedApplication;
        app.SafeInvokeOnMainThread(() =>
        {
            alert = alertFunc();
            var top = Platform.GetCurrentUIViewController();
            if (alert.PreferredStyle == UIAlertControllerStyle.ActionSheet && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                var x = top.View.Bounds.Width / 2;
                var y = top.View.Bounds.Bottom;
                var rect = new CGRect(x, y, 0, 0);
#if __IOS__
                alert.PopoverPresentationController.SourceView = top.View;
                alert.PopoverPresentationController.SourceRect = rect;
                alert.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Unknown;
#endif
            }
            top.PresentViewController(alert, true, null);
        });
        return new DisposableAction(() => app.SafeInvokeOnMainThread(() => alert.DismissViewController(true, null)));
    }

    protected virtual IDisposable Present(UIViewController controller)
    {
        var app = UIApplication.SharedApplication;
        var top = Platform.GetCurrentUIViewController();

        app.InvokeOnMainThread(() => top.PresentViewController(controller, true, null));
        return new DisposableAction(() => app.SafeInvokeOnMainThread(() => controller.DismissViewController(true, null)));
    }
}