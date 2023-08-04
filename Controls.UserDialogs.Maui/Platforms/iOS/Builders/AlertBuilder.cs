using Foundation;

using Microsoft.Maui.Platform;

using UIKit;

namespace Controls.UserDialogs.Maui;

public class AlertBuilder
{
    protected AlertConfig Config { get; }

    public AlertBuilder(AlertConfig config)
    {
        Config = config;
    }

    public virtual UIAlertController Build()
    {
        var alert = UIAlertController.Create("", "", UIAlertControllerStyle.Alert);

        alert.AddAction(GetOkAction(Config));

        if (Config.Title is not null)
        {
            alert.SetValueForKey(GetTitle(Config), new NSString("attributedTitle"));
        }

        if (Config.Message is not null)
        {
            alert.SetValueForKey(GetMessage(Config), new NSString("attributedMessage"));
        }

        if (Config.UserInterfaceStyle is not null)
        {
            alert.OverrideUserInterfaceStyle = Config.UserInterfaceStyle.Value.ToNative();
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
        titleFont ??= UIFont.SystemFontOfSize(config.TitleFontSize, UIFontWeight.Bold);

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
        messageFont ??= UIFont.SystemFontOfSize(config.MessageFontSize);

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