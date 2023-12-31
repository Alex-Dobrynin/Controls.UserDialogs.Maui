﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;

using AndroidX.AppCompat.App;

namespace Controls.UserDialogs.Maui;

public abstract class AbstractAppCompatDialogFragment<T> : AppCompatDialogFragment where T : class
{
    public T Config { get; set; }

    public override void OnSaveInstanceState(Bundle bundle)
    {
        base.OnSaveInstanceState(bundle);
        ConfigStore.Instance.Store(bundle, this.Config);
    }

    public override Dialog OnCreateDialog(Bundle bundle)
    {
        Dialog dialog = null;
        if (this.Config is null && !ConfigStore.Instance.Contains(bundle))
        {
            this.ShowsDialog = false;
            this.Dismiss();
        }
        else
        {
            this.Config ??= ConfigStore.Instance.Pop<T>(bundle);
            dialog = this.CreateDialog(this.Config);
            this.SetDialogDefaults(dialog);
        }
        return dialog;
    }

    protected virtual void SetDialogDefaults(Dialog dialog)
    {
        dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
        dialog.SetCancelable(false);
        dialog.SetCanceledOnTouchOutside(false);
        dialog.KeyPress += this.OnKeyPress;
    }

    public override void OnDetach()
    {
        base.OnDetach();
        if (this.Dialog is not null)
            this.Dialog.KeyPress -= this.OnKeyPress;
    }

    protected virtual void OnKeyPress(object sender, DialogKeyEventArgs args)
    {
    }

    protected abstract Dialog CreateDialog(T config);
    protected AppCompatActivity AppCompatActivity => this.Activity as AppCompatActivity;
}