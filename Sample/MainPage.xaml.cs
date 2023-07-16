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

            UserDialogs.Instance.Alert("sjbknsdkbvlsdbds", "Title", "Cancel");

            //UserDialogs.Instance.ShowLoading("aghah", MaskType.Gradient);
            //var dialog = UserDialogs.Instance.Loading("Some progress", "Great job");

            //await Task.Delay(1000);
            //dialog.Update(new HudDialogConfig { PercentComplete = 0, Title = "Some progress", Message = "some message", OnCancel = () => { }, CancelText = "Cancel" });
            //await Task.Delay(1);
            //dialog.Update(new HudDialogConfig { PercentComplete = 100, Title = "Some progress", Message = "some message", OnCancel = () => { }, CancelText = "Cancel" });
            //await Task.Delay(5000);
            //dialog.Update(new HudDialogConfig { PercentComplete = 80, Title = "Some progress" });
            //await Task.Delay(2000);
            //dialog.Update(new HudDialogConfig { PercentComplete = 70, Title = "Some progress" });
            //await Task.Delay(2000);
            //dialog.Update(new HudDialogConfig { PercentComplete = 60, Title = "Some progress" });
            //await Task.Delay(2000);
            //dialog.Update(new HudDialogConfig { Image = "dotnet_bot.png", Message = "Some progress", Title = "some shit happend", MaskType = MaskType.Clear });

            //await Task.Delay(2000);

            //UserDialogs.Instance.HideHud();

            //UserDialogs.Instance.ShowSnackbar("this is message", "dotnet_bot.png", null, new SnackbarAction() { Icon = "dotnet_bot.png", Text = "Cancel", Action = () => { } });

            //var config = new ActionSheetConfig()
            //{
            //    UseBottomSheet = true,
            //    Destructive = new ActionSheetOption("Destroy", () => { }, "dotnet_bot.png"),
            //    Cancel = new ActionSheetOption("Cancel", () => { }, "dotnet_bot.png"),
            //    Message = "dfhdfjzdj dfh nld ldfnh d",
            //    Title = "ShsilnAH sh",
            //    Icon = "dotnet_bot.png",
            //    Options = new ActionSheetOption[]
            //    {
            //        new ActionSheetOption("ALgnsdlh sdh s", () => { }, "dotnet_bot.png"),
            //        new ActionSheetOption("LILsl.jh", () => { }, "dotnet_bot.png"),
            //        new ActionSheetOption("IAEShldfblo", () => { }, "dotnet_bot.png"),
            //    }
            //};

            //UserDialogs.Instance.ActionSheet(config);

            //var res = await UserDialogs.Instance.ActionSheetAsync("this is some message", "Title", "Cancel", "destroy", "dotnet_bot.png", false, default, "ASljsnd", "Asgunsbd", "Aglish");

            //UserDialogs.Instance.Alert(new AlertConfig() 
            //{ Action = () => { Console.WriteLine("aaaa"); }, Message = "this is some message", Title="Title", Icon="dotnet_bot.png", OkText="Cancel" });
        }
    }
}