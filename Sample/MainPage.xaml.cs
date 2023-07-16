using Maui.Controls.UserDialogs;

namespace Sample
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            await UserDialogs.Instance.AlertAsync("sjbknsdkbvlsdbds", "Title", "Cancel");
            await UserDialogs.Instance.ConfirmAsync("sjbknsdkbvlsdbds", "Title", "Cancel", "Cancel");

            UserDialogs.Instance.ShowLoading("aghah", null, MaskType.Gradient);
            await Task.Delay(2000);

            UserDialogs.Instance.HideHud();

            UserDialogs.Instance.ShowToast(new ToastConfig() { Icon = "dotnet_bot.png", Message = "agasgahadhs" });
            await Task.Delay(3000);
            UserDialogs.Instance.ShowSnackbar(
                new SnackbarConfig()
                {
                    Icon = "dotnet_bot.png",
                    Message = "agasgahadhs",
                    Action = new SnackbarAction()
                    {
                        Action = () => { },
                        Icon = "dotnet_bot.png",
                        Text = "sdhhfgd",
                        ShowCountDown = true
                    }
                });
            await Task.Delay(3000);

            var config = new ActionSheetConfig()
            {
                UseBottomSheet = true,
                Destructive = new ActionSheetOption("Destroy", () => { }, "dotnet_bot.png"),
                Cancel = new ActionSheetOption("Cancel", () => { }, "dotnet_bot.png"),
                Message = "dfhdfjzdj dfh nld ldfnh d",
                Title = "ShsilnAH sh",
                Icon = "dotnet_bot.png",
                Options = new ActionSheetOption[]
                {
                    new ActionSheetOption("ALgnsdlh sdh s", () => { }, "dotnet_bot.png"),
                    new ActionSheetOption("LILsl.jh", () => { }, "dotnet_bot.png"),
                    new ActionSheetOption("IAEShldfblo", () => { }, "dotnet_bot.png"),
                }
            };

            UserDialogs.Instance.ActionSheet(config);
        }
    }
}