using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using itmit.asb.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LcPage : ContentPage
	{
		public LcPage ()
		{
			InitializeComponent ();
			BindingContext = new LcViewModel(App.User);
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