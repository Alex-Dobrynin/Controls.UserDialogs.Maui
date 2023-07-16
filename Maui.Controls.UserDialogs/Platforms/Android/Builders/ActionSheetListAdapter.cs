using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

using Microsoft.Maui.Platform;

using static Maui.Controls.UserDialogs.Extensions;

using View = Android.Views.View;

namespace Maui.Controls.UserDialogs;

public class ActionSheetListAdapter : ArrayAdapter<ActionSheetOption>
{
    readonly ActionSheetConfig _config;

    public ActionSheetListAdapter(Context context, int resource, int textViewResourceId, ActionSheetConfig config)
        : base(context, resource, textViewResourceId, config.Options)
    {
        _config = config;
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
        textView.SetPadding(DpToPixels(20), 0, DpToPixels(20), 0);

        if (item.Icon is not null)
        {
            var imgId = MauiApplication.Current.GetDrawableId(_config.Icon);
            var img = MauiApplication.Current.GetDrawable(imgId);
            img.ScaleTo(24);

            textView.SetCompoundDrawables(img, null, null, null);
            textView.CompoundDrawablePadding = DpToPixels(10);
        }

        view.Foreground = Extensions.GetSelectableItemForeground(this.Context);

        return view;
    }
}