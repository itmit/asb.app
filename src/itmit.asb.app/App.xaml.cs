using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using itmit.asb.app.Views.Guard;
using Matcha.BackgroundService;
using Realms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace itmit.asb.app
{
	public partial class App : Application
	{
		private readonly ILocationUpdatesService _updatesService = DependencyService.Get<ILocationUpdatesService>();

		#region .ctor
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

			StartBackgroundService(new TimeSpan(0, 0, 0, 5));

			MainPage = new AlarmPage();
		}
		#endregion

		public void StartBackgroundService(TimeSpan timeSpan)
		{
			_updatesService.StartService();
		}

		#region Properties
		public static User User
		{
			get
			{
				var con = RealmConfiguration.DefaultConfiguration;
				con.SchemaVersion = 2;
				return Realm.GetInstance(con).All<User>().SingleOrDefault();
			}
		}
		#endregion

		public void Logout()
		{
			var realm = Realm.GetInstance();
			using (var transaction = realm.BeginWrite())
			{
				realm.RemoveAll<User>();
				transaction.Commit();
			}

			_updatesService.StopService();

			Current.MainPage = new LoginPage();
		}

		public static void Call(string number)
		{
			try
			{
				PhoneDialer.Open(number);
			}
			catch (ArgumentNullException anEx)
			{
				Debug.WriteLine(anEx);
			}
			catch (FeatureNotSupportedException ex)
			{
				Debug.WriteLine(ex);

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		#region Overrided
		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnStart()
		{ 
			// Handle when your app starts
		}
		#endregion
	}
}
