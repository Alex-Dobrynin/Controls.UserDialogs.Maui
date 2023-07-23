using Foundation;

using Microsoft.Maui.Platform;

using UIKit;

namespace Controls.UserDialogs.Maui;

public class AlertBuilder
{
    public virtual UIAlertController Build(AlertConfig config)
    {
        var alert = UIAlertController.Create("", "", UIAlertControllerStyle.Alert);

        alert.AddAction(GetOkAction(config));

        alert.SetValueForKey(GetTitle(config), new NSString("attributedTitle"));

        alert.SetValueForKey(GetMessage(config), new NSString("attributedMessage"));

        if (config.UserInterfaceStyle is not null)
        {
            alert.OverrideUserInterfaceStyle = config.UserInterfaceStyle.Value.ToNative();
        }

        return alert;
    }

    protected virtual NSAttributedString GetTitle(AlertConfig config)
    {
        UIFont titleFont = null;
        if (config.TitleFontFamily is not null)
        {
            titleFont = UIFont.FromName(config.TitleFontFamily, config.TitleFontSize);
        }
        if (titleFont is null) titleFont = UIFont.SystemFontOfSize(config.TitleFontSize, UIFontWeight.Bold);

        var attributedString = new NSMutableAttributedString(config.Title, titleFont, config.TitleColor?.ToPlatform());

        return attributedString;
    }

    protected virtual NSAttributedString GetMessage(AlertConfig config)
    {
        UIFont messageFont = null;
        if (config.FontFamily is not null)
        {
            messageFont = UIFont.FromName(config.FontFamily, config.MessageFontSize);
        }
        if (messageFont is null) messageFont = UIFont.SystemFontOfSize(config.MessageFontSize);

        var attributedString = new NSMutableAttributedString(config.Message, messageFont, config.MessageColor?.ToPlatform());

        return attributedString;
    }

    protected virtual UIAlertAction GetOkAction(AlertConfig config)
    {
        var action = UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.Action?.Invoke());

        if (config.PositiveButtonTextColor is not null)
        {
            action.SetValueForKey(config.PositiveButtonTextColor.ToPlatform(), new NSString("titleTextColor"));
        }

        return action;
    }
}