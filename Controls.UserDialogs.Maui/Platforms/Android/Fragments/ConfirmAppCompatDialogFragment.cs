using Android.App;
using Android.Content;
using Android.Views;

namespace Controls.UserDialogs.Maui;

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

    protected override Dialog CreateDialog(ConfirmConfig config) => new ConfirmBuilder(this.AppCompatActivity, config).BuildAppCompat();
}