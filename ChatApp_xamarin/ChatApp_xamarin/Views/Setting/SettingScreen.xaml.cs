using ChatApp_xamarin.ViewModels.Setting;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_xamarin.Views.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingScreen : ContentPage
    {
        public SettingScreen()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var vm = (SettingViewModel)this.BindingContext;
            vm.GetCurrentUserCM.Execute(null);

            vm.IsDark = Preferences.Get("isDark", false);
            vm.ISVN = Preferences.Get("isVN", false);
            vm.IsSilent = Preferences.Get("isSilent", false);
        }

    }
}