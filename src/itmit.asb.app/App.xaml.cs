using System;
using System.Diagnostics;
using System.Linq;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using itmit.asb.app.Views.Guard;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Realms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace itmit.asb.app
{
	public partial class App : Application
	{
		#region Data
		#region Fields
		private readonly ILocationUpdatesService _updatesService = DependencyService.Get<ILocationUpdatesService>();
		#endregion
		#endregion

		#region .ctor
		public App()
		{
			InitializeComponent();

			DependencyService.Register<IAuthService>();


			if (User == null)
			{
				MainPage = new NavigationPage(new LoginPage());
				return;
			}

			if (User.IsGuard)
			{
				MainPage = new GuardMainPage();

				return;
			}

			MainPage = new AlarmPage();
		}

		private async void CheckLocationPermission()
		{
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
			if (status != PermissionStatus.Granted)
			{
				await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
			}
		}
		#endregion

		#region Properties
		public static User User
		{
			get
			{
				var con = RealmConfiguration.DefaultConfiguration;
				con.SchemaVersion = 7;
				return Realm.GetInstance(con)
							.All<User>()
							.SingleOrDefault();
			}
		}
		#endregion

		#region Public
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

		public void Logout()
		{
			var con = RealmConfiguration.DefaultConfiguration;
			con.SchemaVersion = 7;
			var realm = Realm.GetInstance(con);
			using (var transaction = realm.BeginWrite())
			{
				realm.RemoveAll<User>();
				transaction.Commit();
			}

			_updatesService.StopService();

			Current.MainPage = new NavigationPage(new LoginPage());
		}

		public void StartBackgroundService(TimeSpan timeSpan)
		{
			_updatesService.StartService();
		}
		#endregion

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
			if (Device.RuntimePlatform == Device.iOS)
			{
				CheckLocationPermission();
			}

			// Handle when your app starts
			if (User != null)
			{
				StartBackgroundService(new TimeSpan(0, 0, 0, 5));
			}
		}
		#endregion
	}
}
