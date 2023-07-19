namespace Controls.UserDialogs.Maui;

public partial class UserDialogs
{
    static IUserDialogs _currentInstance;
    public static IUserDialogs Instance
    {
        get
        {
            if (_currentInstance is null)
                throw new ArgumentException("[Controls.UserDialogs.Maui] You must call UseUserDialogs() in your MauiProgram for initialization");

            return _currentInstance;
        }
        set => _currentInstance = value;
    }

    public static partial MauiAppBuilder UseUserDialogs(this MauiAppBuilder builder, Action configure = null);
}