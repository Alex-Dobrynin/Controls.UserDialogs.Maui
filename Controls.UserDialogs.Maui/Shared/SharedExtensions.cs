namespace Controls.UserDialogs.Maui;

public static class SharedExtensions
{
    public static void SafeInvokeOnMainThread(Action action)
    {
        if (MainThread.IsMainThread)
        {
            SafeAction();
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(SafeAction);
        }

        void SafeAction()
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}