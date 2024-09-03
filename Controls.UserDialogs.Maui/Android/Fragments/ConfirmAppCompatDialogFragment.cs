using Android.App;
using Android.Content;
using Android.Views;

namespace Controls.UserDialogs.Maui;

public class ConfirmAppCompatDialogFragment : AbstractAppCompatDialogFragment<ConfirmConfig>
{
    protected override void OnKeyPress(object? sender, DialogKeyEventArgs args)
    {
        base.OnKeyPress(sender, args);
        if (args.KeyCode != Keycode.Back)
            return;

        args.Handled = true;
        Config?.Action?.Invoke(false);
        Dismiss();
    }

    protected override Dialog CreateDialog(ConfirmConfig config) => new ConfirmBuilder(AppCompatActivity, config).BuildAppCompat();
}