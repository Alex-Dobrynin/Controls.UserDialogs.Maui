namespace Controls.UserDialogs.Maui;

public interface IHudDialog : IDisposable
{
    void Update(string? message = null, int percentComplete = -1, string? image = null, string? cancelText = null, bool show = true, MaskType? maskType = null, Action? cancel = null);
    void Update(HudDialogConfig config);

    void Show();
    void Hide();
}