using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;

using AndroidX.AppCompat.App;

namespace Controls.UserDialogs.Maui;

public abstract class AbstractAppCompatDialogFragment<T> : AppCompatDialogFragment where T : class
{
    public T? Config { get; set; }

    public override void OnSaveInstanceState(Bundle bundle)
    {
        base.OnSaveInstanceState(bundle);
        ConfigStore.Instance.Store(bundle, Config!);
    }

    public override Dialog OnCreateDialog(Bundle? bundle)
    {
        Dialog? dialog = null;
        if (Config is null && !ConfigStore.Instance.Contains(bundle))
        {
            ShowsDialog = false;
            Dismiss();
        }
        else
        {
            Config ??= ConfigStore.Instance.Pop<T>(bundle!);
            dialog = CreateDialog(Config);
            SetDialogDefaults(dialog);
        }
        return dialog;
    }

    protected virtual void SetDialogDefaults(Dialog dialog)
    {
        dialog.Window!.SetSoftInputMode(SoftInput.StateVisible);
        dialog.SetCancelable(false);
        dialog.SetCanceledOnTouchOutside(false);
        dialog.KeyPress += OnKeyPress;
    }

    public override void OnDetach()
    {
        base.OnDetach();
        if (Dialog is not null)
            Dialog.KeyPress -= OnKeyPress;
    }

    protected virtual void OnKeyPress(object? sender, DialogKeyEventArgs args)
    {
    }

    protected abstract Dialog CreateDialog(T config);
    protected AppCompatActivity AppCompatActivity => (Activity as AppCompatActivity)!;
}