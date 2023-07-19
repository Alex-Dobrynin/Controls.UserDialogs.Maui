namespace Maui.Controls.UserDialogs;

public interface IUserDialogs
{
    public IHudDialog CurrentHudDialog { get; }
    public bool IsHudShowing => CurrentHudDialog is not null;

    IDisposable Alert(string message, string title = null, string okText = null, string icon = null);
    IDisposable Alert(AlertConfig config);
    Task AlertAsync(string message, string title = null, string okText = null, string icon = null, CancellationToken? cancelToken = null);
    Task AlertAsync(AlertConfig config, CancellationToken? cancelToken = null);

    IDisposable Confirm(ConfirmConfig config);
    Task<bool> ConfirmAsync(string message, string title = null, string okText = null, string cancelText = null, string icon = null, CancellationToken? cancelToken = null);
    Task<bool> ConfirmAsync(ConfirmConfig config, CancellationToken? cancelToken = null);

    IDisposable ActionSheet(ActionSheetConfig config);
    Task<string> ActionSheetAsync(string message = null, string title = null, string cancel = null, string destructive = null, string icon = null, bool useBottomSheet = true, CancellationToken? cancelToken = null, params string[] buttons);


    void ShowLoading(string message = null, string title = null, MaskType? maskType = null);
    void HideHud();
    IHudDialog Loading(string message = null, string cancelText = null, bool show = true, MaskType? maskType = null, Action onCancel = null);
    IHudDialog Progress(string message = null, string cancelText = null, bool show = true, MaskType? maskType = null, Action onCancel = null);
    IHudDialog ShowHudImage(string image, string message = null, string cancelText = null, bool show = true, MaskType? maskType = null, Action onCancel = null);
    IHudDialog CreateOrUpdateHud(HudDialogConfig config);

    IDisposable ShowToast(string message, string icon = null, TimeSpan? dismissTimer = null);
    IDisposable ShowToast(ToastConfig cfg);
    IDisposable ShowSnackbar(string message, string icon = null, TimeSpan? dismissTimer = null, string actionText = null, string actionIcon = null, bool showCountDown = false, Action<SnackbarActionType> action = null);
    IDisposable ShowSnackbar(SnackbarConfig config);
    Task<SnackbarActionType> ShowSnackbarAsync(string message, string icon = null, TimeSpan? dismissTimer = null, string actionText = null, string actionIcon = null, bool showCountDown = false, CancellationToken? cancelToken = null);
    Task<SnackbarActionType> ShowSnackbarAsync(SnackbarConfig config, CancellationToken? cancelToken = null);
}