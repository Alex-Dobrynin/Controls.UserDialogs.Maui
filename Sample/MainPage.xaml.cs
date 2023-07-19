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

            //await UserDialogs.Instance.AlertAsync("sjbknsdkbvlsdbds", "Title", "Ok");
            //await UserDialogs.Instance.ConfirmAsync("sjbknsdkbvlsdbds", "Title", "Ok", "Cancel");

            var p = UserDialogs.Instance.ShowHudImage("dotnet_bot.png", "dskjbnskdbsalv");

            await Task.Delay(3000);

            for (int i = 0; i < 100; i++)
            {
                p.Update(i.ToString(), i);
                await Task.Delay(1000);
            }

            //await Task.Delay(4000);
            //p.Update("rjvrijr", "ejvmrjb", 50);
            //await Task.Delay(4000);

            //p.Update("ckjfevrbr", "ejvmrjb", 60);
            //await Task.Delay(4000);

            //p.Update("kce dckevefb", "ejvmrjb", 70);

            await Task.Delay(10000);

            UserDialogs.Instance.HideHud();

            //UserDialogs.Instance.ShowToast(new ToastConfig() { Icon = "dotnet_bot.png", Message = "agasgahadhskjdsg skljh dfkbs dbkjsdbdfkjb djfb dslj sjfb dlfb odjfb d j sjdf vod bjdf bjsvodf bjd bks" });
            //UserDialogs.Instance.ShowSnackbar(new SnackbarConfig()
            //{
            //    Icon = "dotnet_bot.png",
            //    Message = "a kdjbgd sogd of bodfnbljndfbl elfg ledgli jdh elfigj ergij eflg df gelg erlgjerl elerjg erlhkhj er elrkj erhhl kerjhh ",
            //    ActionText = "Cancel",
            //    ActionIcon = "dotnet_bot.png",
            //    Action = type =>
            //    {

            //    }
            //});
            //await Task.Delay(3000);
            //UserDialogs.Instance.ShowSnackbar(
            //    new SnackbarConfig()
            //    {
            //        Icon = "dotnet_bot.png",
            //        Message = "agasgahadhs",
            //        Action = new SnackbarAction()
            //        {
            //            Action = () => { },
            //            Icon = "dotnet_bot.png",
            //            Text = "sdhhfgd",
            //            ShowCountDown = true
            //        }
            //    });
            //await Task.Delay(3000);

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

            //UserDialogs.Instance.ActionSheet(config);
        }
    }
}