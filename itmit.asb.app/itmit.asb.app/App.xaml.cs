using System;
using itmit.asb.app.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using itmit.asb.app.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace itmit.asb.app
{
    public partial class App : Application
	{
		private static User _user;

		public static User User
		{
			get => _user;
			set
			{
				if (value.Token.Equals(string.Empty))
				{
					return;
				}
				_user = value;
			}
		}

		public App()
        {
            InitializeComponent();

			MainPage = new LoginPage();
            //MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
