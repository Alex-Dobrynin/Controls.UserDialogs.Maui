namespace Controls.UserDialogs.Maui;

public static partial class UserDialogs
{
    public static partial MauiAppBuilder UseUserDialogs(this MauiAppBuilder builder, Action configure = null)
    {
        Instance = new UserDialogsImplementation();

        configure?.Invoke();

        return builder;
    }
}