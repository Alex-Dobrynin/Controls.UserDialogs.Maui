namespace Controls.UserDialogs.Maui;

public partial class UserDialogsImplementation : IDisposable
{
    public virtual partial IDisposable Alert(AlertConfig config)
    {
        return this;
    }

    public virtual partial IDisposable Confirm(ConfirmConfig config)
    {
        return this;
    }

    public virtual partial IDisposable ActionSheet(ActionSheetConfig config)
    {
        return this;
    }

    public virtual partial IDisposable ShowToast(ToastConfig config)
    {
        return this;
    }

    public virtual partial IDisposable ShowSnackbar(SnackbarConfig config)
    {
        return this;
    }

    protected virtual partial IHudDialog? CreateHudInstance(HudDialogConfig config)
    {
        return CurrentHudDialog;
    }
}