using System;
using System.Linq;
using System.Runtime.CompilerServices;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using itmit.asb.app.Views;
using itmit.asb.app.Views.Guard;
using Realms;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace itmit.asb.app
{
    public partial class App : Application
	{
		public static User User =>
			Realm.GetInstance()
				 .All<User>()
				 .SingleOrDefault();

		public App()
        {
            InitializeComponent();

			DependencyService.Register<IAuthService>();
			
			if (User == null)
			{
				MainPage = new LoginPage();
				return;
			}

			if (User.IsGuard)
			{
				MainPage = new NavigationPage(new GuardMainPage());
				return;
			}

			MainPage = new alarm();
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
