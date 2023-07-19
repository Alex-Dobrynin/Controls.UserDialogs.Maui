namespace Maui.Controls.UserDialogs;

public interface IHudDialog : IDisposable
{
    void Update(string message = null, int percentComplete = -1, string image = null, string cancelText = null, bool show = true, MaskType? maskType = null, Action onCancel = null);
    void Update(HudDialogConfig config);

    void Show();
    void Hide();
}