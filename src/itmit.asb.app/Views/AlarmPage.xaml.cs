using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using itmit.asb.app.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class alarm : ContentPage
	{
		public alarm ()
		{
			InitializeComponent ();
			
			BindingContext = new AlarmViewModel();
		}

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new alarm());
        }

        private void ImageButton_Clicked_1(object sender, EventArgs e)
        {
           // void Call("");
           // return;
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

        //public void Call(string number)
        //{
        //    try
        //    {
        //        PhoneDialer.Open(number);
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        Exception_alarm.Text = "Number was null or white space";
        //    }
        //    catch (FeatureNotSupportedException)
        //    {
        //        Exception_alarm.Text = "Phone Dialer is not supported on this device.";
        //    }
        //    catch (Exception)
        //    {
        //        Exception_alarm.Text = "Other error has occurred";
        //    }
        //}
    }
}