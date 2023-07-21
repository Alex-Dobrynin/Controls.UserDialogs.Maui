using Controls.UserDialogs.Maui;

namespace Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.Alert("This is Alert dialog", "Alert dialog", "Understand", "dotnet_bot.png");
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await UserDialogs.Instance.AlertAsync("This is asyn Alert dialog", "Async Alert dialog", "Understand", "dotnet_bot.png");
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            UserDialogs.Instance.Confirm("This is Confirm dialog", "Confirm dialog", "Understand", "Nope", "dotnet_bot.png", res =>
            {

            });
        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            var res = await UserDialogs.Instance.ConfirmAsync("This is Async Confirm dialog", "Async Confirm dialog", "Understand", "Cancel", "dotnet_bot.png");
        }

        private void Button_Clicked_4(object sender, EventArgs e)
        {
#if IOS
            UserDialogs.Instance.Alert("Action sheet is not supported on ios", "Warning", "Understand", "dotnet_bot.png");
#else
            var config = new ActionSheetConfig()
            {
                UseBottomSheet = false,
                Destructive = new ActionSheetOption("Destroy", () => { }, "dotnet_bot.png"),
                Cancel = new ActionSheetOption("Cancel", () => { }, "dotnet_bot.png"),
                Title = "Action sheet",
                Icon = "dotnet_bot.png",
                Options = new ActionSheetOption[]
                {
                    new ActionSheetOption("First option", () => { }, "dotnet_bot.png"),
                    new ActionSheetOption("Second option", () => { }, "dotnet_bot.png"),
                    new ActionSheetOption("Third option", () => { }, "dotnet_bot.png"),
                }
            };

            UserDialogs.Instance.ActionSheet(config);
#endif
        }

        private async void Button_Clicked_5(object sender, EventArgs e)
        {
#if IOS
            UserDialogs.Instance.Alert("Async Action sheet is not supported on ios", "Warning", "Understand", "dotnet_bot.png");
#else
            var res = await UserDialogs.Instance.ActionSheetAsync(
                "not supported", 
                "Async Action sheet", 
                "Cancel", 
                "Destroy", 
                "dotnet_bot.png", 
                false, 
                default, 
                "First option", 
                "Second option", 
                "Third option"
                );
#endif
        }

        private void Button_Clicked_6(object sender, EventArgs e)
        {
            var config = new ActionSheetConfig()
            {
                UseBottomSheet = true,
                Destructive = new ActionSheetOption("Destroy", () => { }, "dotnet_bot.png"),
                Cancel = new ActionSheetOption("Cancel", () => { }, "dotnet_bot.png"),
                Title = "Bottom Action sheet",
                Message = "This is Bottom Action sheet",
                Icon = "dotnet_bot.png",
                Options = new ActionSheetOption[]
                {
                    new ActionSheetOption("First option", () => { }, "dotnet_bot.png"),
                    new ActionSheetOption("Second option", () => { }, "dotnet_bot.png"),
                    new ActionSheetOption("Third option", () => { }, "dotnet_bot.png"),
                }
            };

            UserDialogs.Instance.ActionSheet(config);
        }

        private async void Button_Clicked_7(object sender, EventArgs e)
        {
            var res = await UserDialogs.Instance.ActionSheetAsync(
                "This is Async Bottom Action sheet",
                "Async Bottom Action sheet",
                "Cancel",
                "Destroy",
                "dotnet_bot.png",
                true,
                default,
                "First option",
                "Second option",
                "Third option"
                );
        }

        private async void Button_Clicked_8(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Loading HUD");
            await Task.Delay(3000);
            UserDialogs.Instance.HideHud();

            var hudDialog = UserDialogs.Instance.Loading("Another loading HUD");
            await Task.Delay(3000);
            hudDialog.Update("Previous loading but with action", -1, null, "Cancel", cancel: () =>
            {

            });
            await Task.Delay(10000);
            hudDialog.Dispose();
            // or
            //UserDialogs.Instance.HideHud();
        }

        private async void Button_Clicked_9(object sender, EventArgs e)
        {
            var hudDialog = UserDialogs.Instance.Progress("Progress HUD");
            await Task.Delay(3000);

            for (int i = 0; i < 100; i++)
            {
                hudDialog.Update("Previous Progress but with action", i, null, "Cancel", cancel: () =>
                {

                });
                await Task.Delay(500);
            }
            hudDialog.Dispose();
            // or
            //UserDialogs.Instance.HideHud();
        }

        private async void Button_Clicked_10(object sender, EventArgs e)
        {
            var hudDialog = UserDialogs.Instance.ShowHudImage("dotnet_bot.png", "Image HUD");
            await Task.Delay(3000);
            hudDialog.Dispose();

            hudDialog = UserDialogs.Instance.ShowHudImage("dotnet_bot.png", "Another Image HUD");
#if ANDROID
            await Task.Delay(3000);
            hudDialog.Update("Previous Image but with action", -1, "dotnet_bot.png", "Cancel", cancel: () =>
            {

            });
#endif
            await Task.Delay(10000);
            hudDialog.Dispose();
            // or
            //UserDialogs.Instance.HideHud();
        }

        private void Button_Clicked_11(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowToast(new ToastConfig()
            {
                Icon = "dotnet_bot.png",
                Message = "This is toast notification"
            });
        }

        private void Button_Clicked_12(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowSnackbar(new SnackbarConfig()
            {
                Icon = "dotnet_bot.png",
                Message = "This is snackbar",
                ActionText = "Cancel",
                ActionIcon = "dotnet_bot.png",
                ShowCountDown = true,
                Action = type =>
                {

                }
            });
        }

        private async void Button_Clicked_13(object sender, EventArgs e)
        {
            var res = await UserDialogs.Instance.ShowSnackbarAsync(new SnackbarConfig()
            {
                Icon = "dotnet_bot.png",
                Message = "This is a Snackbar",
                ActionText = "Understand",
                ActionIcon = "dotnet_bot.png"
            });
        }
    }
}