using UIKit;

namespace Maui.Controls.UserDialogs;

public class AlertBuilder
{
    public virtual UIAlertController Build(AlertConfig config)
    {
        var alert = UIAlertController.Create(config.Title ?? string.Empty, config.Message, UIAlertControllerStyle.Alert);
        alert.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.Action?.Invoke()));
        if (config.UserInterfaceStyle is not null)
        {
            alert.OverrideUserInterfaceStyle = config.UserInterfaceStyle.Value.ToNative();
        }

        

        return alert;
    }
}