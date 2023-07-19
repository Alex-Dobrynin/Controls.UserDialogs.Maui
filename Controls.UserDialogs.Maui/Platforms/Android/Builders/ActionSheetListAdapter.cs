using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

using Microsoft.Maui.Platform;

using static Controls.UserDialogs.Maui.Extensions;

using View = Android.Views.View;

namespace Controls.UserDialogs.Maui;

public class ActionSheetListAdapter : ArrayAdapter<ActionSheetOption>
{
    private readonly ActionSheetBuilder _builder;
    private readonly ActionSheetConfig _config;
    private readonly Typeface _typeface;

    public ActionSheetListAdapter(Context context, int resource, int textViewResourceId, ActionSheetBuilder builder, ActionSheetConfig config, Typeface typeface)
        : base(context, resource, textViewResourceId, config.Options)
    {
        _builder = builder;
        _config = config;
        _typeface = typeface;
    }

    public override View GetView(int position, View convertView, ViewGroup parent)
    {
        //Use base class to create the View
        var view = base.GetView(position, convertView, parent);
        var textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);

        var item = this._config.Options.ElementAt(position);

        textView.Text = item.Text;
        textView.SetTextSize(ComplexUnitType.Sp, (float)_config.ActionSheetOptionFontSize);
        if (_config.ActionSheetOptionTextColor is not null)
        {
            textView.SetTextColor(_config.ActionSheetOptionTextColor.ToPlatform());
        }
        textView.SetPadding(DpToPixels(_builder.Padding.Left), 0, DpToPixels(_builder.Padding.Right), 0);
        textView.SetTypeface(_typeface, TypefaceStyle.Normal);

        if (item.Icon is not null)
        {
            var imgId = MauiApplication.Current.GetDrawableId(_config.Icon);
            var img = MauiApplication.Current.GetDrawable(imgId);
            img.ScaleTo(_builder.OptionIconSize);

            textView.SetCompoundDrawables(img, null, null, null);
            textView.CompoundDrawablePadding = DpToPixels(_builder.OptionIconPadding);
        }

        view.Foreground = Extensions.GetSelectableItemForeground(this.Context);

        return view;
    }
}