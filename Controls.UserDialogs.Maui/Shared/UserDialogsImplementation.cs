namespace Controls.UserDialogs.Maui;

public partial class UserDialogsImplementation : IUserDialogs
{
    const string _noAction = "Action should not be set as async will not use it";

    public IHudDialog? CurrentHudDialog { get; protected set; }

    public virtual partial IDisposable Alert(AlertConfig config);
    public virtual partial IDisposable Confirm(ConfirmConfig config);
    public virtual partial IDisposable ActionSheet(ActionSheetConfig config);
    public virtual partial IDisposable ShowToast(ToastConfig config);
    public virtual partial IDisposable ShowSnackbar(SnackbarConfig config);

    protected virtual partial IHudDialog CreateHudInstance(HudDialogConfig config);

    public virtual IDisposable Alert(string message, string? title = null, string? okText = null, string? icon = null, Action? action = null)
    {
        return Alert(new AlertConfig
        {
            Message = message,
            Title = title,
            Icon = icon,
            Action = action,
            OkText = okText ?? AlertConfig.DefaultOkText
        });
    }

    public virtual Task AlertAsync(string message, string? title = null, string? okText = null, string? icon = null, CancellationToken cancelToken = default)
    {
        return AlertAsync(new AlertConfig
        {
            Message = message,
            Title = title,
            Icon = icon,
            OkText = okText ?? AlertConfig.DefaultOkText
        }, cancelToken);
    }

    public virtual async Task AlertAsync(AlertConfig config, CancellationToken cancelToken = default)
    {
        if (config.Action is not null)
            throw new ArgumentException(_noAction);

        var tcs = new TaskCompletionSource<object?>();
        config.SetAction(() => tcs.TrySetResult(null));

        var disp = Alert(config);
        using (cancelToken.Register(() => Cancel(disp, tcs)))
        {
            await tcs.Task;
        }
    }

    public virtual IDisposable Confirm(string message, string? title = null, string? okText = null, string? cancelText = null, string? icon = null, Action<bool>? action = null)
    {
        return Confirm(new ConfirmConfig
        {
            Message = message,
            Title = title,
            Icon = icon,
            Action = action,
            CancelText = cancelText ?? (!ConfirmConfig.DefaultUseYesNo ? ConfirmConfig.DefaultCancelText : ConfirmConfig.DefaultNo),
            OkText = okText ?? (!ConfirmConfig.DefaultUseYesNo ? ConfirmConfig.DefaultOkText : ConfirmConfig.DefaultYes)
        });
    }

    public virtual Task<bool> ConfirmAsync(string message, string? title = null, string? okText = null, string? cancelText = null, string? icon = null, CancellationToken cancelToken = default)
    {
        return ConfirmAsync(new ConfirmConfig
        {
            Message = message,
            Title = title,
            Icon = icon,
            CancelText = cancelText ?? (!ConfirmConfig.DefaultUseYesNo ? ConfirmConfig.DefaultCancelText : ConfirmConfig.DefaultNo),
            OkText = okText ?? (!ConfirmConfig.DefaultUseYesNo ? ConfirmConfig.DefaultOkText : ConfirmConfig.DefaultYes)
        }, cancelToken);
    }

    public virtual async Task<bool> ConfirmAsync(ConfirmConfig config, CancellationToken cancelToken = default)
    {
        if (config.Action is not null)
            throw new ArgumentException(_noAction);

        var tcs = new TaskCompletionSource<bool>();
        config.SetAction(x => tcs.TrySetResult(x));

        var disp = Confirm(config);
        using (cancelToken.Register(() => Cancel(disp, tcs)))
        {
            return await tcs.Task;
        }
    }

    public virtual async Task<string> ActionSheetAsync(string? message = null, string? title = null, string? cancel = null, string? destructive = null, string? icon = null, bool useBottomSheet = true, CancellationToken cancelToken = default, params string[] buttons)
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

        var disp = ActionSheet(cfg);
        using (cancelToken.Register(disp.Dispose))
        {
            return await tcs.Task;
        }
    }

    public virtual void ShowLoading(string? message = null, MaskType? maskType = null)
    {
        Loading(message, null, true, maskType, null);
    }

    public virtual void HideHud()
    {
        CurrentHudDialog?.Dispose();
        CurrentHudDialog = null;
    }

    public virtual IHudDialog Loading(string? message = null, string? cancelText = null, bool show = true, MaskType? maskType = null, Action? cancel = null)
        => CreateOrUpdateHud(new HudDialogConfig
        {
            Message = message,
            AutoShow = show,
            CancelText = cancelText ?? HudDialogConfig.DefaultCancelText,
            MaskType = maskType ?? HudDialogConfig.DefaultMaskType,
            PercentComplete = -1,
            Cancel = cancel
        });

    public virtual IHudDialog Progress(string? message = null, string? cancelText = null, bool show = true, MaskType? maskType = null, Action? cancel = null)
        => CreateOrUpdateHud(new HudDialogConfig
        {
            Message = message,
            AutoShow = show,
            CancelText = cancelText ?? HudDialogConfig.DefaultCancelText,
            MaskType = maskType ?? HudDialogConfig.DefaultMaskType,
            PercentComplete = 0,
            Cancel = cancel
        });

    public virtual IHudDialog ShowHudImage(string image, string? message = null, string? cancelText = null, bool show = true, MaskType? maskType = null, Action? cancel = null)
       => CreateOrUpdateHud(new HudDialogConfig
       {
           Message = message,
           AutoShow = show,
           CancelText = cancelText ?? HudDialogConfig.DefaultCancelText,
           MaskType = maskType ?? HudDialogConfig.DefaultMaskType,
           Image = image,
           Cancel = cancel
       });

    public virtual IHudDialog CreateOrUpdateHud(HudDialogConfig config)
    {
        try
        {
            if (CurrentHudDialog is not null) CurrentHudDialog.Update(config);
            else CurrentHudDialog = CreateHudInstance(config);

            return CurrentHudDialog;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public virtual IDisposable ShowToast(string message, string? icon = null, TimeSpan? duration = null)
        => ShowToast(new ToastConfig()
        {
            Message = message,
            Icon = icon,
            Duration = duration ?? ToastConfig.DefaultDuration
        });

    public virtual IDisposable ShowSnackbar(string message, string? actionText = null, string? icon = null, TimeSpan? duration = null, string? actionIcon = null, bool showCountDown = false, Action<SnackbarActionType>? action = null)
        => action is not null
            ? ShowSnackbar(new SnackbarConfig()
            {
                Message = message,
                Icon = icon,
                Duration = duration ?? SnackbarConfig.DefaultDuration,
                Action = action,
                ActionIcon = actionIcon,
                ActionText = actionText ?? SnackbarConfig.DefaultActionText,
                ShowCountDown = showCountDown
            })
            : ShowToast(new ToastConfig()
            {
                Message = message,
                Icon = icon,
                Duration = duration ?? ToastConfig.DefaultDuration
            });

    public virtual Task<SnackbarActionType> ShowSnackbarAsync(string message, string? actionText = null, string? icon = null, TimeSpan? duration = null, string? actionIcon = null, bool showCountDown = false, CancellationToken cancelToken = default)
        => ShowSnackbarAsync(new SnackbarConfig()
        {
            Message = message,
            Icon = icon,
            Duration = duration ?? SnackbarConfig.DefaultDuration,
            ActionIcon = actionIcon,
            ActionText = actionText ?? SnackbarConfig.DefaultActionText,
            ShowCountDown = showCountDown
        }, cancelToken);

    public virtual async Task<SnackbarActionType> ShowSnackbarAsync(SnackbarConfig config, CancellationToken cancelToken = default)
    {
        if (config.Action is not null)
            throw new ArgumentException(_noAction);

        var tcs = new TaskCompletionSource<SnackbarActionType>();
        config.SetAction(x => tcs.TrySetResult(x));

        var disp = ShowSnackbar(config);
        using (cancelToken.Register(() => Cancel(disp, tcs)))
        {
            return await tcs.Task;
        }
    }

    protected virtual void Cancel<TResult>(IDisposable disp, TaskCompletionSource<TResult> tcs)
    {
        disp.Dispose();
        tcs.TrySetCanceled();
    }
}