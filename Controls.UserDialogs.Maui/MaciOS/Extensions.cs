using CoreGraphics;

using UIKit;

namespace Controls.UserDialogs.Maui;

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
            UserInterfaceStyle.Light => UIUserInterfaceStyle.Light,
            UserInterfaceStyle.Dark => UIUserInterfaceStyle.Dark,
            _ => UIUserInterfaceStyle.Unspecified,
        };
    }

    public static Position ToNative(this ToastPosition position)
    {
        return position switch
        {
            ToastPosition.Top => Position.Top,
            _ => Position.Bottom,
        };
    }

    public static UIImage ScaleTo(this UIImage image, double newSize)
    {
        double width = image.Size.Width;
        double height = image.Size.Height;

        CGSize size;
        var ratio = width / height;
        if (width < height)
        {
            size = new CGSize(newSize * ratio, newSize);
        }
        else size = new CGSize(newSize, newSize / ratio);

        var renderer = new UIGraphicsImageRenderer(size);
        var resizedImage = renderer.CreateImage((UIGraphicsImageRendererContext context) =>
        {
            image.Draw(new CGRect(CGPoint.Empty, size));
        }).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);

        return resizedImage;
    }

    public static UIWindow? GetKeyWindow()
    {
        var window = GetActiveScene()?.Windows.FirstOrDefault(w => w.IsKeyWindow);

        return window;
    }

    public static UIWindowScene? GetActiveScene()
    {
        var connectedScene = UIApplication.SharedApplication.ConnectedScenes
            .OfType<UIWindowScene>()
            .OrderBy(x => x.ActivationState)
            .FirstOrDefault(x => x.ActivationState
                is UISceneActivationState.ForegroundActive
                or UISceneActivationState.ForegroundInactive);

        return connectedScene;
    }

    public static BigTed.MaskType ToNative(this MaskType maskType)
    {
        return maskType switch
        {
            MaskType.Black => BigTed.MaskType.Black,
            MaskType.Clear => BigTed.MaskType.Clear,
            MaskType.Gradient => BigTed.MaskType.Black,
            MaskType.None => BigTed.MaskType.None,
            _ => throw new ArgumentException("Invalid Mask Type"),
        };
    }
}