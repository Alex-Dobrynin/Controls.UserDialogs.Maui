using Foundation;

using Microsoft.Maui.Platform;

using UIKit;

namespace Controls.UserDialogs.Maui;

public class ActionSheetBuilder
{
    public static double DefaultOptionIconSize { get; set; } = 24;
    public static double DefaultDestructiveIconSize { get; set; } = 24;
    public static double DefaultCancelIconSize { get; set; } = 24;

    public double OptionIconSize { get; set; } = DefaultOptionIconSize;
    public double DestructiveIconSize { get; set; } = DefaultDestructiveIconSize;
    public double CancelIconSize { get; set; } = DefaultCancelIconSize;
    protected ActionSheetConfig Config { get; }

    public ActionSheetBuilder(ActionSheetConfig config)
    {
        Config = config;
    }

    public virtual UIAlertController Build()
    {
        var alert = UIAlertController.Create("", "", UIAlertControllerStyle.ActionSheet);

        Config.Options.ToList().ForEach(o => alert.AddAction(GetOptionAction(o)));

        alert.AddAction(GetDestructiveAction());
        alert.AddAction(GetCancelAction());

        if (Config.Title is not null)
        {
            alert.SetValueForKey(GetTitle(), new NSString("attributedTitle"));
        }

        if (Config.Message is not null)
        {
            alert.SetValueForKey(GetMessage(), new NSString("attributedMessage"));
        }

        if (Config.UserInterfaceStyle is not null)
        {
            alert.OverrideUserInterfaceStyle = Config.UserInterfaceStyle.Value.ToNative();
        }

        return alert;
    }

    protected virtual NSAttributedString GetTitle()
    {
        UIFont titleFont = null;
        if (Config.TitleFontFamily is not null)
        {
            titleFont = UIFont.FromName(Config.TitleFontFamily, Config.TitleFontSize);
        }
        titleFont ??= UIFont.SystemFontOfSize(Config.TitleFontSize, UIFontWeight.Bold);

        var attributedString = new NSMutableAttributedString(Config.Title, titleFont, Config.TitleColor?.ToPlatform());

        return attributedString;
    }

    protected virtual NSAttributedString GetMessage()
    {
        UIFont messageFont = null;
        if (Config.MessageFontFamily is not null)
        {
            messageFont = UIFont.FromName(Config.MessageFontFamily, Config.MessageFontSize);
        }
        messageFont ??= UIFont.SystemFontOfSize(Config.MessageFontSize);

        var attributedString = new NSMutableAttributedString(Config.Message, messageFont, Config.MessageColor?.ToPlatform());

        return attributedString;
    }

    protected virtual UIAlertAction GetOptionAction(ActionSheetOption option)
    {
        var action = UIAlertAction.Create(option.Text, UIAlertActionStyle.Default, x => option.Action?.Invoke());

        if (Config.ActionSheetOptionTextColor is not null)
        {
            action.SetValueForKey(Config.ActionSheetOptionTextColor.ToPlatform(), new NSString("titleTextColor"));
        }

        if (option.Icon is not null)
        {
            var img = new UIImage(option.Icon).ScaleTo(OptionIconSize);
            action.SetValueForKey(img, new NSString("image"));
        }

        return action;
    }

    protected virtual UIAlertAction GetDestructiveAction()
    {
        var action = UIAlertAction.Create(Config.Destructive.Text, UIAlertActionStyle.Destructive, x => Config.Destructive.Action?.Invoke());

        if (Config.DestructiveButtonTextColor is not null)
        {
            action.SetValueForKey(Config.DestructiveButtonTextColor.ToPlatform(), new NSString("titleTextColor"));
        }

        if (Config.Destructive.Icon is not null)
        {
            var img = new UIImage(Config.Destructive.Icon).ScaleTo(DestructiveIconSize);
            action.SetValueForKey(img, new NSString("image"));
        }

        return action;
    }

    protected virtual UIAlertAction GetCancelAction()
    {
        var action = UIAlertAction.Create(Config.Cancel.Text, UIAlertActionStyle.Cancel, x => Config.Cancel.Action?.Invoke());

        if (Config.NegativeButtonTextColor is not null)
        {
            action.SetValueForKey(Config.NegativeButtonTextColor.ToPlatform(), new NSString("titleTextColor"));
        }

        if (Config.Cancel.Icon is not null)
        {
            var img = new UIImage(Config.Cancel.Icon).ScaleTo(CancelIconSize);
            action.SetValueForKey(img, new NSString("image"));
        }

        return action;
    }
}