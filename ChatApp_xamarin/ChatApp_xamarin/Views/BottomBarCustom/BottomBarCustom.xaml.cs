
using ChatApp_xamarin.Services;
using ChatApp_xamarin.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_xamarin.Views.BottomBarCustom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomBarCustom : TabbedPage
    {
        public BottomBarCustom()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            if (GlobalData.ins.currentUser != null) return;
            var id = Preferences.Get("currentUser", null);
            GlobalData.ins.currentUser = await UserService.ins.GetUserById(id);
        }
    }
}