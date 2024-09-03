namespace Controls.UserDialogs.Maui;

public interface IUserDialogs
{
    public IHudDialog? CurrentHudDialog { get; }
    public bool IsHudShowing => CurrentHudDialog is not null;

    IDisposable Alert(string message, string? title = null, string? okText = null, string? icon = null, Action? action = null);
    IDisposable Alert(AlertConfig config);
    Task AlertAsync(string message, string? title = null, string? okText = null, string? icon = null, CancellationToken cancelToken = default);
    Task AlertAsync(AlertConfig config, CancellationToken cancelToken = default);

    IDisposable Confirm(string message, string? title = null, string? okText = null, string? cancelText = null, string? icon = null, Action<bool>? action = null);
    IDisposable Confirm(ConfirmConfig config);
    Task<bool> ConfirmAsync(string message, string? title = null, string? okText = null, string? cancelText = null, string? icon = null, CancellationToken cancelToken = default);
    Task<bool> ConfirmAsync(ConfirmConfig config, CancellationToken cancelToken = default);

    IDisposable ActionSheet(ActionSheetConfig config);
    Task<string> ActionSheetAsync(string? message = null, string? title = null, string? cancel = null, string? destructive = null, string? icon = null, bool useBottomSheet = true, CancellationToken cancelToken = default, params string[] buttons);


    void ShowLoading(string? message = null, MaskType? maskType = null);
    void HideHud();
    IHudDialog Loading(string? message = null, string? cancelText = null, bool show = true, MaskType? maskType = null, Action? cancel = null);
    IHudDialog Progress(string? message = null, string? cancelText = null, bool show = true, MaskType? maskType = null, Action? cancel = null);
    IHudDialog ShowHudImage(string image, string? message = null, string? cancelText = null, bool show = true, MaskType? maskType = null, Action? cancel = null);
    IHudDialog CreateOrUpdateHud(HudDialogConfig config);

    IDisposable ShowToast(string message, string? icon = null, TimeSpan? duration = null);
    IDisposable ShowToast(ToastConfig cfg);
    IDisposable ShowSnackbar(string message, string? actionText = null, string? icon = null, TimeSpan? duration = null, string? actionIcon = null, bool showCountDown = false, Action<SnackbarActionType>? action = null);
    IDisposable ShowSnackbar(SnackbarConfig config);
    Task<SnackbarActionType> ShowSnackbarAsync(string message, string? actionText = null, string? icon = null, TimeSpan? duration = null, string? actionIcon = null, bool showCountDown = false, CancellationToken cancelToken = default);
    Task<SnackbarActionType> ShowSnackbarAsync(SnackbarConfig config, CancellationToken cancelToken = default);
}