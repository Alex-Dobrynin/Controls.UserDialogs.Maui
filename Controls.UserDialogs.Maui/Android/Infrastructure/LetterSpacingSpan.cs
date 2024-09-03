using Android.Text;
using Android.Text.Style;

namespace Controls.UserDialogs.Maui;

public class LetterSpacingSpan : MetricAffectingSpan
{
    private float _letterSpacing;

    public LetterSpacingSpan(float letterSpacing)
    {
        _letterSpacing = letterSpacing;
    }

    public float getLetterSpacing()
    {
        return _letterSpacing;
    }

    public override void UpdateDrawState(TextPaint? ds)
    {
        Apply(ds);
    }

    public override void UpdateMeasureState(TextPaint paint)
    {
        Apply(paint);
    }

    private void Apply(TextPaint? paint)
    {
        if (paint is null) return;

        paint.LetterSpacing = _letterSpacing;
    }
}