using Android.App;

using AndroidX.AppCompat.App;

using Google.Android.Material.Snackbar;

using Platform = Microsoft.Maui.ApplicationModel.Platform;

namespace Controls.UserDialogs.Maui;

public partial class UserDialogsImplementation
{
    public static string FragmentTag { get; set; } = "UserDialogs";

    #region Dialogs

    public virtual partial IDisposable Alert(AlertConfig config)
    {
        var activity = Platform.CurrentActivity!;
        if (activity is AppCompatActivity compat)
            return ShowDialog<AlertAppCompatDialogFragment, AlertConfig>(compat, config);

        return Show(activity, () => new AlertBuilder(activity, config).Build());
    }

    public virtual partial IDisposable Confirm(ConfirmConfig config)
    {
        var activity = Platform.CurrentActivity!;
        if (activity is AppCompatActivity compat)
            return ShowDialog<ConfirmAppCompatDialogFragment, ConfirmConfig>(compat, config);

        return Show(activity, () => new ConfirmBuilder(activity, config).Build());
    }

    public virtual partial IDisposable ActionSheet(ActionSheetConfig config)
    {
        var activity = Platform.CurrentActivity!;
        if (activity is AppCompatActivity compat)
        {
            if (config.UseBottomSheet)
                return ShowDialog<BottomSheetDialogFragment, ActionSheetConfig>(compat, config);

            return ShowDialog<ActionSheetAppCompatDialogFragment, ActionSheetConfig>(compat, config);
        }

        return Show(activity, () => new ActionSheetBuilder(activity, config).Build());
    }

    #endregion

    #region Toasts

    public virtual partial IDisposable ShowToast(ToastConfig config)
    {
        Snackbar? snackbar = null;
        var activity = Platform.CurrentActivity!;
        activity.SafeRunOnUi(() =>
        {
            snackbar = new ToastBuilder(activity, config).Build();

            snackbar.Show();
        });
        return new DisposableAction(() =>
        {
            if (snackbar?.IsShown is true)
                activity.SafeRunOnUi(snackbar.Dismiss);
        });
    }

    public virtual partial IDisposable ShowSnackbar(SnackbarConfig config)
    {
        Snackbar? snackBar = null;
        var activity = Platform.CurrentActivity!;
        activity.SafeRunOnUi(() =>
        {
            snackBar = new SnackbarBuilder(activity, config).Build();

            snackBar.Show();
        });
        return new DisposableAction(() =>
        {
            if (snackBar?.IsShown is true)
                activity.SafeRunOnUi(() =>
                {
                    snackBar.Dismiss();
                    config.Action?.Invoke(SnackbarActionType.Cancelled);
                });
        });
    }

    #endregion

    #region Internals

    protected virtual partial IHudDialog CreateHudInstance(HudDialogConfig config)
    {
        CurrentHudDialog?.Dispose();

        var dialog = new HudDialog(Platform.CurrentActivity!);
        dialog.Update(config);

        return dialog;
    }

    protected virtual IDisposable Show(Activity activity, Func<Dialog> dialogBuilder)
    {
        Dialog? dialog = null;
        activity.SafeRunOnUi(() =>
        {
            dialog = dialogBuilder();
            dialog.Show();
        });
        return new DisposableAction(() =>
        {
            activity.SafeRunOnUi(() => dialog?.Dismiss());
        });
    }

    protected virtual IDisposable ShowDialog<TFragment, TConfig>(AppCompatActivity activity, TConfig config)
        where TFragment : AbstractAppCompatDialogFragment<TConfig>
        where TConfig : class, new()
    {
        TFragment? frag = null;
        activity.SafeRunOnUi(() =>
        {
            frag = (TFragment)Activator.CreateInstance(typeof(TFragment))!;
            frag.Config = config;
            frag.Show(activity.SupportFragmentManager, FragmentTag);
        });
        return new DisposableAction(() =>
        {
            activity.SafeRunOnUi(() => frag?.Dismiss());
        });
    }

    #endregion
}