using itmit.asb.app.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		#region .ctor
		public LoginPage()
		{
			InitializeComponent();
			BindingContext = new LoginViewModel();
            SizeChanged += OnSizeChanged;
        }
        #endregion

        void OnSizeChanged(object sender, EventArgs e)
        {

            if (Application.Current.MainPage.Width >= 600)
            {
                Logo.HeightRequest = 400;
                Login.WidthRequest = 400;
                Login.HorizontalOptions = LayoutOptions.Center;
                Password.WidthRequest = 400;
                Password.HorizontalOptions = LayoutOptions.Center;
            }
            else
            {
                Logo.HeightRequest = 200;
                Login.WidthRequest = Application.Current.MainPage.Width;
                Login.HorizontalOptions = LayoutOptions.Fill;
                Password.WidthRequest = Application.Current.MainPage.Width;
                Password.HorizontalOptions = LayoutOptions.Fill;
            }
        }
    }
}
