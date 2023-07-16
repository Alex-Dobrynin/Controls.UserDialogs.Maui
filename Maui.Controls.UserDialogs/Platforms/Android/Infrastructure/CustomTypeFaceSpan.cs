using Android.Graphics;
using Android.Text;
using Android.Text.Style;

namespace Maui.Controls.UserDialogs;

public class CustomTypeFaceSpan : MetricAffectingSpan
{
    private Typeface _typeface;

    public CustomTypeFaceSpan(Typeface typeface)
    {
        this._typeface = typeface;
    }

    public override void UpdateMeasureState(TextPaint paint)
    {
        paint.SetTypeface(_typeface);
    }

    public override void UpdateDrawState(TextPaint paint)
    {
        paint.SetTypeface(_typeface);
    }
}
