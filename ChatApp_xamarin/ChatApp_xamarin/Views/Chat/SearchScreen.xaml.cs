using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_xamarin.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchScreen : ContentPage
    {
        public SearchScreen()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            searchEntry.Focus();
        }
    }
}