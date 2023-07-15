namespace Maui.Controls.UserDialogs;

public interface IHudDialog : IDisposable
{
    void Update(HudDialogConfig config);

    void Show();
    void Hide();
}