using Android.App;
using Android.Content;
using Android.Views;

namespace Controls.UserDialogs.Maui;

public class ActionSheetAppCompatDialogFragment : AbstractAppCompatDialogFragment<ActionSheetConfig>
{
    protected override void SetDialogDefaults(Dialog dialog)
    {
        base.SetDialogDefaults(dialog);

        dialog.CancelEvent += (sender, args) => this.Config?.Cancel?.Action?.Invoke();

        var cancellable = this.Config.Cancel is not null;
        dialog.SetCancelable(cancellable);
        dialog.SetCanceledOnTouchOutside(cancellable);
    }

    public override void OnCancel(IDialogInterface dialog)
    {
        base.OnCancel(dialog);
        this.Config?.Cancel?.Action?.Invoke();
    }

    public override void Dismiss()
    {
        base.Dismiss();
        this.Config?.Cancel?.Action?.Invoke();
    }

    protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
    {
        base.OnKeyPress(sender, args);

        if (args.KeyCode != Keycode.Back)
            return;

        args.Handled = true;
        this.Dismiss();
    }

    protected override Dialog CreateDialog(ActionSheetConfig config) => new ActionSheetBuilder(this.AppCompatActivity, config).BuildAppCompat();
}