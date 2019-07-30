using System;
using System.Linq;
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
		public static User User
		{
			get;
			set;
		}

		public App()
        {
            InitializeComponent();

			DependencyService.Register<IAuthService>();

			var realm = Realm.GetInstance();
			User user = realm.All<User>().SingleOrDefault();

			if (user == null)
			{
				MainPage = new LoginPage();
				return;
			}

			User = user;
			if (User.IsGuard)
			{
				MainPage = new GuardMainPage();
				return;
			}

			MainPage = new MainPage();
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
