namespace Maui.Controls.UserDialogs;

public partial class UserDialogsImplementation : IUserDialogs
{
    const string _noAction = "Action should not be set as async will not use it";

    public IHudDialog CurrentHudDialog { get; protected set; }

    public virtual partial IDisposable Alert(AlertConfig config);
    public virtual partial IDisposable Confirm(ConfirmConfig config);
    public virtual partial IDisposable ActionSheet(ActionSheetConfig config);
    public virtual partial IDisposable ShowToast(ToastConfig config);
    public virtual partial IDisposable ShowSnackbar(SnackbarConfig config);

    protected virtual partial IHudDialog CreateHudInstance(HudDialogConfig config);

    public virtual IDisposable Alert(string message, string title, string okText, string icon)
    {
        return Alert(new AlertConfig
        {
            Message = message,
            Title = title,
            Icon = icon,
            OkText = okText ?? AlertConfig.DefaultOkText
        });
    }

    public virtual Task AlertAsync(string message, string title, string okText, string icon, CancellationToken? cancelToken = null)
    {
        return AlertAsync(new AlertConfig
        {
            Message = message,
            Title = title,
            Icon = icon,
            OkText = okText ?? AlertConfig.DefaultOkText
        }, cancelToken);
    }

    public async Task AlertAsync(AlertConfig config, CancellationToken? cancelToken = null)
    {
        if (config.Action is not null)
            throw new ArgumentException(_noAction);

        var tcs = new TaskCompletionSource<object>();
        config.SetAction(() => tcs.TrySetResult(null));

        var disp = Alert(config);
        using (cancelToken?.Register(() => Cancel(disp, tcs)))
        {
            await tcs.Task;
        }
    }

    public virtual Task<bool> ConfirmAsync(string message, string title, string okText, string cancelText, string icon, CancellationToken? cancelToken = null)
    {
        return this.ConfirmAsync(new ConfirmConfig
        {
            Message = message,
            Title = title,
            Icon = icon,
            CancelText = cancelText ?? (!ConfirmConfig.DefaultUseYesNo ? ConfirmConfig.DefaultCancelText : ConfirmConfig.DefaultNo),
            OkText = okText ?? (!ConfirmConfig.DefaultUseYesNo ? ConfirmConfig.DefaultOkText : ConfirmConfig.DefaultYes)
        }, cancelToken);
    }

    public virtual async Task<bool> ConfirmAsync(ConfirmConfig config, CancellationToken? cancelToken = null)
    {
        if (config.Action is not null)
            throw new ArgumentException(_noAction);

        var tcs = new TaskCompletionSource<bool>();
        config.SetAction(x => tcs.TrySetResult(x));

        var disp = this.Confirm(config);
        using (cancelToken?.Register(() => Cancel(disp, tcs)))
        {
            return await tcs.Task;
        }
    }

    public virtual async Task<string> ActionSheetAsync(string message, string title, string cancel, string destructive, string icon, bool useBottomSheet, CancellationToken? cancelToken = null, params string[] buttons)
    {
        var tcs = new TaskCompletionSource<string>();
        var cfg = new ActionSheetConfig()
        {
            Message = message,
            Title = title,
            UseBottomSheet = useBottomSheet,
            Icon = icon,
        };

        // you must have a cancel option for actionsheetasync
        if (cancel is null)
            throw new ArgumentException("You must have a cancel option for the async version");

        cfg.SetCancel(cancel, () => tcs.TrySetResult(cancel));
        if (destructive is not null)
            cfg.SetDestructive(destructive, () => tcs.TrySetResult(destructive));

        foreach (var btn in buttons)
            cfg.Add(btn, () => tcs.TrySetResult(btn));

        var disp = this.ActionSheet(cfg);
        using (cancelToken?.Register(disp.Dispose))
        {
            return await tcs.Task;
        }
    }

    public virtual void ShowLoading(string title, string message, MaskType? maskType)
    {
        this.Loading(title, message, null, true, maskType, null);
    }

    public virtual void HideHud()
    {
        CurrentHudDialog?.Dispose();
        CurrentHudDialog = null;
    }

    public virtual IHudDialog Loading(string title, string message, string cancelText, bool show, MaskType? maskType, Action onCancel)
        => this.CreateOrUpdateHud(new HudDialogConfig
        {
            Title = title ?? HudDialogConfig.DefaultTitle,
            Message = message,
            AutoShow = show,
            CancelText = cancelText ?? HudDialogConfig.DefaultCancelText,
            MaskType = maskType ?? HudDialogConfig.DefaultMaskType,
            PercentComplete = -1,
            OnCancel = onCancel
        });

    public virtual IHudDialog Progress(string title, string message, string cancelText, bool show, MaskType? maskType, Action onCancel)
        => this.CreateOrUpdateHud(new HudDialogConfig
        {
            Title = title ?? HudDialogConfig.DefaultTitle,
            Message = message,
            AutoShow = show,
            CancelText = cancelText ?? HudDialogConfig.DefaultCancelText,
            MaskType = maskType ?? HudDialogConfig.DefaultMaskType,
            PercentComplete = 0,
            OnCancel = onCancel
        });

    public virtual IHudDialog ShowHudImage(string image, string title, string message, string cancelText, bool show, MaskType? maskType, Action onCancel)
       => this.CreateOrUpdateHud(new HudDialogConfig
       {
           Title = title,
           Message = message,
           AutoShow = show,
           CancelText = cancelText ?? HudDialogConfig.DefaultCancelText,
           MaskType = maskType ?? HudDialogConfig.DefaultMaskType,
           Image = image,
           OnCancel = onCancel
       });

    public virtual IHudDialog CreateOrUpdateHud(HudDialogConfig config)
    {
        if (CurrentHudDialog is not null) CurrentHudDialog.Update(config);
        else CurrentHudDialog = this.CreateHudInstance(config);

        return CurrentHudDialog;
    }

    public virtual IDisposable ShowToast(string message, string icon, TimeSpan? dismissTimer)
        => this.ShowToast(new ToastConfig()
        {
            Message = message,
            Icon = icon,
            Duration = dismissTimer ?? ToastConfig.DefaultDuration
        });

    public virtual IDisposable ShowSnackbar(string message, string icon = null, TimeSpan? dismissTimer = null, SnackbarAction action = null)
        => this.ShowSnackbar(new SnackbarConfig()
        {
            Message = message,
            Icon = icon,
            Duration = dismissTimer ?? SnackbarConfig.DefaultDuration,
            Action = action
        });

    static void Cancel<TResult>(IDisposable disp, TaskCompletionSource<TResult> tcs)
    {
        disp.Dispose();
        tcs.TrySetCanceled();
    }
}