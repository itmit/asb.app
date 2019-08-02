using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new alarm());
        }

        private void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new alarm());
        }

        private void ImageButton_Clicked_2(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LcPage());
        }

        private void ImageButton_Clicked_3(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new AboutPage());
        }

        private void ImageButton_Clicked_4(object sender, EventArgs e)
        {

        }
    }
}