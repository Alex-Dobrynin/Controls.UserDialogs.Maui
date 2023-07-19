using Android.App;

using AndroidX.AppCompat.App;

using Google.Android.Material.Snackbar;

using Platform = Microsoft.Maui.ApplicationModel.Platform;

namespace Maui.Controls.UserDialogs;

public partial class UserDialogsImplementation
{
    public static string FragmentTag { get; set; } = "UserDialogs";

    #region Alert Dialogs

    public virtual partial IDisposable Alert(AlertConfig config)
    {
        var activity = Platform.CurrentActivity;
        if (activity is AppCompatActivity act)
            return this.ShowDialog<AlertAppCompatDialogFragment, AlertConfig>(act, config);

        return this.Show(activity, () => new AlertBuilder().Build(activity, config));
    }

    public virtual partial IDisposable Confirm(ConfirmConfig config)
    {
        var activity = Platform.CurrentActivity;
        if (activity is AppCompatActivity act)
            return this.ShowDialog<ConfirmAppCompatDialogFragment, ConfirmConfig>(act, config);

        return this.Show(activity, () => new ConfirmBuilder().Build(activity, config));
    }

    public virtual partial IDisposable ActionSheet(ActionSheetConfig config)
    {
        var activity = Platform.CurrentActivity;
        if (activity is AppCompatActivity act)
        {
            if (config.UseBottomSheet)
                return this.ShowDialog<BottomSheetDialogFragment, ActionSheetConfig>(act, config);

            return this.ShowDialog<ActionSheetAppCompatDialogFragment, ActionSheetConfig>(act, config);
        }

        return this.Show(activity, () => new ActionSheetBuilder().Build(activity, config));
    }

    #endregion

    #region Toasts

    public virtual partial IDisposable ShowToast(ToastConfig config)
    {
        Snackbar snackBar = null;
        var activity = Platform.CurrentActivity;
        activity.SafeRunOnUi(() =>
        {
            snackBar = new ToastBuilder().Build(activity, config);

            snackBar.Show();
        });
        return new DisposableAction(() =>
        {
            if (snackBar.IsShown)
                activity.SafeRunOnUi(snackBar.Dismiss);
        });
    }

    public virtual partial IDisposable ShowSnackbar(SnackbarConfig config)
    {
        Snackbar snackBar = null;
        var activity = Platform.CurrentActivity;
        activity.SafeRunOnUi(() =>
        {
            snackBar = new SnackbarBuilder().Build(activity, config);

            snackBar.Show();

            var timer = new System.Timers.Timer
            {
                Interval = config.Duration.TotalMilliseconds,
                AutoReset = false
            };

            var endOfAnimation = DateTime.Now + config.Duration;
            timer.Elapsed += (s, a) =>
            {
                timer.Stop();
                activity.SafeRunOnUi(() => config.Action?.Invoke(SnackbarActionType.Timeout));
            };
            timer.Start();
        });
        return new DisposableAction(() =>
        {
            if (snackBar.IsShown)
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
        var dialog = new HudDialog();
        dialog.Update(config);

        return dialog;
    }

    protected virtual IDisposable Show(Activity activity, Func<Dialog> dialogBuilder)
    {
        Dialog dialog = null;
        activity.SafeRunOnUi(() =>
        {
            dialog = dialogBuilder();
            dialog.Show();
        });
        return new DisposableAction(() =>
            activity.SafeRunOnUi(dialog.Dismiss)
        );
    }

    protected virtual IDisposable ShowDialog<TFragment, TConfig>(AppCompatActivity activity, TConfig config)
        where TFragment : AbstractAppCompatDialogFragment<TConfig>
        where TConfig : class, new()
    {
        TFragment frag = null;
        activity.SafeRunOnUi(() =>
        {
            frag = (TFragment)Activator.CreateInstance(typeof(TFragment));
            frag.Config = config;
            frag.Show(activity.SupportFragmentManager, FragmentTag);
        });
        return new DisposableAction(() =>
            activity.SafeRunOnUi(frag.Dismiss)
        );
    }

    #endregion
}