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


    void ShowLoading(string title = null, string message = null, MaskType? maskType = null);
    void HideHud();
    IHudDialog Loading(string title = null, string message = null, string cancelText = null, bool show = true, MaskType? maskType = null, Action onCancel = null);
    IHudDialog Progress(string title = null, string message = null, string cancelText = null, bool show = true, MaskType? maskType = null, Action onCancel = null);
    IHudDialog ShowHudImage(string image, string title = null, string message = null, string cancelText = null, bool show = true, MaskType? maskType = null, Action onCancel = null);
    IHudDialog CreateOrUpdateHud(HudDialogConfig config);

    IDisposable ShowToast(string message, string icon = null, TimeSpan? dismissTimer = null);
    IDisposable ShowToast(ToastConfig cfg);
    IDisposable ShowSnackbar(string message, string icon = null, TimeSpan? dismissTimer = null, SnackbarAction action = null);
    IDisposable ShowSnackbar(SnackbarConfig config);
}