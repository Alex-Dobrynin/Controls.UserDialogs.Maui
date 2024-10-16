using Android.App;
using Android.Content;
using Android.Graphics.Drawables;

using Platform = Microsoft.Maui.ApplicationModel.Platform;

namespace Controls.UserDialogs.Maui;

public static class Extensions
{
    public static void ScaleTo(this Drawable drawable, double newSize)
    {
        double width = drawable.IntrinsicWidth;
        double height = drawable.IntrinsicHeight;

        var ratio = width / height;
        if (width < height)
        {
            drawable.SetBounds(0, 0, DpToPixels(newSize * ratio), DpToPixels(newSize));
        }
        else drawable.SetBounds(0, 0, DpToPixels(newSize), DpToPixels(newSize / ratio));
    }

    public static int DpToPixels(double number)
    {
        var density = Platform.CurrentActivity!.Resources!.DisplayMetrics!.Density;

        return (int)(density * number);
    }

    public static void SafeRunOnUi(this Activity activity, Action action) => activity.RunOnUiThread(() =>
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    });

    public static AndroidHUD.MaskType ToNative(this MaskType maskType)
    {
        switch (maskType)
        {
            case MaskType.Black:
                return AndroidHUD.MaskType.Black;

            case MaskType.Clear:
                return AndroidHUD.MaskType.Clear;

            case MaskType.Gradient:
                Console.WriteLine("Warning - Gradient mask type is not supported on android");
                return AndroidHUD.MaskType.Black;

            case MaskType.None:
                return AndroidHUD.MaskType.None;

            default:
                throw new ArgumentException("Invalid Mask Type");
        }
    }

    public static Drawable? GetSelectableItemForeground(Context context)
    {
        var typedArray = context.ObtainStyledAttributes([Android.Resource.Attribute.SelectableItemBackground]);
        var ripple = typedArray.GetDrawable(0) as RippleDrawable;

        ripple?.SetColor(new([[]], [Colors.Gray.WithAlpha(0.5f).ToInt()]));

        return ripple;
    }

    public static bool IsRTL()
    {
        var layoutDirection = Platform.CurrentActivity!.Resources!.Configuration!.LayoutDirection;
        return layoutDirection == Android.Views.LayoutDirection.Rtl;
    }
}