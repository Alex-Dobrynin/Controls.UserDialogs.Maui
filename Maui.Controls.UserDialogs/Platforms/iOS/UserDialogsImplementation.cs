using CoreGraphics;

using UIKit;

namespace Maui.Controls.UserDialogs;

public partial class UserDialogsImplementation
{
    public virtual partial IDisposable Alert(AlertConfig config) => this.Present(() =>
    {
        return new AlertBuilder().Build(config);
    });

    public virtual partial IDisposable Confirm(ConfirmConfig config) => this.Present(() =>
    {
        return new ConfirmBuilder().Build(config);
    });

    public virtual partial IDisposable ActionSheet(ActionSheetConfig config) => this.Present(() =>
    {
        return new ActionSheetBuilder().Build(config);
    });

    public virtual partial IDisposable ShowToast(ToastConfig config)
    {
        var window = GetDefaultWindow();

        // Calculate the desired frame for your view
        var margin = 20; // Adjust the margin value as needed
        var viewWidth = window.Bounds.Width - (margin * 2);
        var viewHeight = 50; // Adjust the height as needed

        // Create your view and set its frame
        var customView = new UIView(new CGRect(0, 0, viewWidth, viewHeight));
        customView.BackgroundColor = UIColor.Red; // Set your desired background color

        // Set the view's center to the bottom of the window
        customView.Center = new CGPoint(window.Bounds.GetMidX(), window.Bounds.Height - (viewHeight / 2) - margin);

        // Add the view to the window
        window.AddSubview(customView);

        return null;
    }

    public virtual partial IDisposable ShowSnackbar(SnackbarConfig config)
    {
        return null;
    }

    protected virtual partial IHudDialog CreateHudInstance(HudDialogConfig config)
    {
        return null;
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

    public static UIWindow GetDefaultWindow()
    {
        UIWindow window = null;

        if (OperatingSystem.IsMacCatalystVersionAtLeast(15) || OperatingSystem.IsIOSVersionAtLeast(15))
        {
            foreach (var scene in UIApplication.SharedApplication.ConnectedScenes)
            {
                if (scene is UIWindowScene windowScene)
                {
                    window = windowScene.KeyWindow;

                    window ??= windowScene?.Windows?.LastOrDefault();
                }
            }
        }
        else if (OperatingSystem.IsMacCatalystVersionAtLeast(13) || OperatingSystem.IsIOSVersionAtLeast(13))
        {
            window = UIApplication.SharedApplication.Windows?.LastOrDefault();
        }
        else
        {
            window = UIApplication.SharedApplication.KeyWindow
                ?? UIApplication.SharedApplication.Windows?.LastOrDefault();
        }

        return window;
    }
}