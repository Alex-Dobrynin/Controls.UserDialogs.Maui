using Foundation;

using Microsoft.Maui.Platform;

using UIKit;

namespace Controls.UserDialogs.Maui;

public class ConfirmBuilder
{
    protected ConfirmConfig Config { get; }

    public ConfirmBuilder(ConfirmConfig config)
    {
        Config = config;
    }

    public virtual UIAlertController Build()
    {
        var alert = UIAlertController.Create("", "", UIAlertControllerStyle.Alert);

        alert.AddAction(GetCancelAction(Config));
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

    protected virtual NSAttributedString GetTitle(ConfirmConfig config)
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

    protected virtual NSAttributedString GetMessage(ConfirmConfig config)
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

    protected virtual UIAlertAction GetOkAction(ConfirmConfig config)
    {
        var action = UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.Action?.Invoke(true));

        if (config.PositiveButtonTextColor is not null)
        {
            action.SetValueForKey(config.PositiveButtonTextColor.ToPlatform(), new NSString("titleTextColor"));
        }

        return action;
    }

    protected virtual UIAlertAction GetCancelAction(ConfirmConfig config)
    {
        var action = UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.Action?.Invoke(false));

        if (config.NegativeButtonTextColor is not null)
        {
            action.SetValueForKey(config.NegativeButtonTextColor.ToPlatform(), new NSString("titleTextColor"));
        }

        return action;
    }
}