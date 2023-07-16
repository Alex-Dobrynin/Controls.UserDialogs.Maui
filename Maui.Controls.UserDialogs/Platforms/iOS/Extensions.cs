using UIKit;

namespace Maui.Controls.UserDialogs;

public static class Extensions
{
    public static void SafeInvokeOnMainThread(this UIApplication app, Action action) => app.InvokeOnMainThread(() =>
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    });

    public static UIUserInterfaceStyle ToNative(this UserInterfaceStyle style)
    {
        return style switch
        {
            UserInterfaceStyle.Unspecified => UIUserInterfaceStyle.Unspecified,
            UserInterfaceStyle.Light => UIUserInterfaceStyle.Light,
            UserInterfaceStyle.Dark => UIUserInterfaceStyle.Dark,
        };
    }
}