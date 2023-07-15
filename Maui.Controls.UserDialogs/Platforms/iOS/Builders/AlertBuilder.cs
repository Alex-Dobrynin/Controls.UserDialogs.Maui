using Microsoft.Maui.Platform;

using UIKit;

namespace Maui.Controls.UserDialogs;

public class AlertBuilder
{
    public virtual UIAlertController Build(AlertConfig config)
    {
        var alert = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
        alert.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.Action?.Invoke()));

        var backgroundView = alert.View.Subviews.FirstOrDefault()?.Subviews.FirstOrDefault();
        if (backgroundView != null)
        {
            backgroundView.BackgroundColor = AlertConfig.BackgroundColor.ToPlatform();
        }

        return alert;
    }
}