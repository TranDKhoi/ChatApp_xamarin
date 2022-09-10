
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}