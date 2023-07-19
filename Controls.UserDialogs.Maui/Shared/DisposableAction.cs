namespace Controls.UserDialogs.Maui;

public class DisposableAction : IDisposable
{
    readonly Action _action;

    public DisposableAction(Action action)
    {
        this._action = action;
    }

    public void Dispose()
    {
        this._action();
        GC.SuppressFinalize(this);
    }
}