﻿using Android.App;
using Android.Content;
using Android.Views;

namespace Controls.UserDialogs.Maui;

public class AlertAppCompatDialogFragment : AbstractAppCompatDialogFragment<AlertConfig>
{
    protected override void OnKeyPress(object? sender, DialogKeyEventArgs args)
    {
        base.OnKeyPress(sender, args);

        if (args.KeyCode != Keycode.Back)
            return;

        args.Handled = true;
        Config?.Action?.Invoke();
        Dismiss();
    }

    protected override Dialog CreateDialog(AlertConfig config) => new AlertBuilder(AppCompatActivity, config).BuildAppCompat();
}