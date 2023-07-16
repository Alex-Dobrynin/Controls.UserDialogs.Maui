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
}