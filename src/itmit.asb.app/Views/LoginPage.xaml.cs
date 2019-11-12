using System;
using itmit.asb.app.ViewModels;
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
			BindingContext = new LoginViewModel(Navigation);
            
            SizeChanged += OnSizeChanged;
		}
        #endregion

        #region Private
        private void OnSizeChanged(object sender, EventArgs e)
		{
			if (Application.Current.MainPage.Width >= 600)
			{
				Logo.HeightRequest = 400;
				Login.WidthRequest = 400;
				Login.HorizontalOptions = LayoutOptions.Center;
				Password.WidthRequest = 400;
				Password.HorizontalOptions = LayoutOptions.Center;
                ForgetPass.WidthRequest = 230;
                Registration.WidthRequest = 230;
			}
			else
			{
				Logo.HeightRequest = 200;
				Login.WidthRequest = Application.Current.MainPage.Width;
				Login.HorizontalOptions = LayoutOptions.Fill;
				Password.WidthRequest = Application.Current.MainPage.Width;
				Password.HorizontalOptions = LayoutOptions.Fill;
                ForgetPass.WidthRequest = default;
                Registration.WidthRequest = default;
            }
		}

        #endregion
    }
}
