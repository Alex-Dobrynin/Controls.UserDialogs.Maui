using Android.App;
using Android.Content;
using Android.Views;

namespace Maui.Controls.UserDialogs;

public class ConfirmAppCompatDialogFragment : AbstractAppCompatDialogFragment<ConfirmConfig>
{
    protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
    {
        base.OnKeyPress(sender, args);
        if (args.KeyCode != Keycode.Back)
            return;

        args.Handled = true;
        this.Config?.Action?.Invoke(false);
        this.Dismiss();
    }

    protected override Dialog CreateDialog(ConfirmConfig config) => new ConfirmBuilder().Build(this.AppCompatActivity, config);
}