using Base64Animator.ViewModels;
using Xamarin.Forms;

namespace Base64Animator
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            ((MainViewModel)(BindingContext)).LunchAnnimation();
        }
    }
}
