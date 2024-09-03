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
    protected ActionSheetBuilder Builder { get; }
    protected ActionSheetConfig Config { get; }

    public ActionSheetListAdapter(Context context, int resource, int textViewResourceId, ActionSheetBuilder builder, ActionSheetConfig config)
        : base(context, resource, textViewResourceId, config.Options)
    {
        Builder = builder;
        Config = config;
    }

    public override View GetView(int position, View? convertView, ViewGroup parent)
    {
        //Use base class to create the View
        var view = base.GetView(position, convertView, parent);
        var textView = view.FindViewById<TextView>(Android.Resource.Id.Text1)!;

        var item = Config.Options.ElementAt(position);

        textView.Text = item.Text;
        textView.SetTextSize(ComplexUnitType.Sp, (float)Config.ActionSheetOptionFontSize);
        if (Config.ActionSheetOptionTextColor is not null)
        {
            textView.SetTextColor(Config.ActionSheetOptionTextColor.ToPlatform());
        }
        textView.SetPadding(DpToPixels(Builder.Padding.Left), 0, DpToPixels(Builder.Padding.Right), 0);

        if (Config.OptionsButtonFontFamily is not null)
        {
            var typeface = Typeface.CreateFromAsset(Context.Assets, Config.OptionsButtonFontFamily);
            textView.SetTypeface(typeface, TypefaceStyle.Normal);
        }

        if (item.Icon is not null)
        {
            var imgId = MauiApplication.Current.GetDrawableId(Config.Icon!);
            var img = MauiApplication.Current.GetDrawable(imgId)!;
            img.ScaleTo(Builder.OptionIconSize);

            textView.SetCompoundDrawables(img, null, null, null);
            textView.CompoundDrawablePadding = DpToPixels(Builder.OptionIconPadding);
        }

        view.Foreground = GetSelectableItemForeground(Context);

        return view;
    }
}