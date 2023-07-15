namespace Maui.Controls.UserDialogs;

public static partial class UserDialogs
{
    public static partial MauiAppBuilder UseUserDialogs(this MauiAppBuilder builder, Action configure = null)
    {
        Instance = new UserDialogsImplementation();

        configure?.Invoke();

        return builder;
    }
}