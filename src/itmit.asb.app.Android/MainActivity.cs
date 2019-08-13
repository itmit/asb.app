using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using ImageCircle.Forms.Plugin.Droid;
using itmit.asb.app.Services;
using Plugin.Permissions;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(AuthService))]
[assembly: Dependency(typeof(BidsService))]

namespace itmit.asb.app.Droid
{
	[Activity(Label = "itmit.asb.app",
		Icon = "@mipmap/icon",
		Theme = "@style/MainTheme",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		#region Data
		#region Consts
		private const int PermissionsRequestAccessCoarseLocation = 100;

		private const int PermissionsRequestAccessFineLocation = 50;

		private const int PermissionsRequestAccessReadStorage = 10;

		private const int PermissionsRequestAccessWriteStorage = 20;

		private const int RequestCheckSettings = 1;
		#endregion
		#endregion

		#region Overrided
		protected override void OnCreate(Bundle savedInstanceState)
		{
            ImageCircleRenderer.Init();

            TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);
			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			Forms.Init(this, savedInstanceState);

			CheckPermissions();

			Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

			DisplayLocationSettingsRequest();
			FormsMaps.Init(this, savedInstanceState);
			LoadApplication(new App());
		}
		#endregion

		#region Private
		private void DisplayLocationSettingsRequest()
		{
			var googleApiClient = new GoogleApiClient.Builder(this).AddApi(LocationServices.API)
																   .Build();
			googleApiClient.Connect();

			var locationRequest = LocationRequest.Create();
			locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
			locationRequest.SetInterval(10000);
			locationRequest.SetFastestInterval(10000 / 2);

			var builder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
			builder.SetAlwaysShow(true);

			var result = LocationServices.SettingsApi.CheckLocationSettings(googleApiClient, builder.Build());
			result.SetResultCallback((LocationSettingsResult callback) =>
			{
				switch (callback.Status.StatusCode)
				{
					case CommonStatusCodes.Success:
					{
						//DoStuffWithLocation();
						break;
					}
					case CommonStatusCodes.ResolutionRequired:
					{
						try
						{
							// Show the dialog by calling startResolutionForResult(), and check the result
							// in onActivityResult().
							callback.Status.StartResolutionForResult(this, RequestCheckSettings);
						}
						catch (IntentSender.SendIntentException e)
						{
						}

						break;
					}
					default:
					{
						// If all else fails, take the user to the android location settings
						StartActivity(new Intent(Settings.ActionLocationSourceSettings));
						break;
					}
				}
			});
		}

		private void CheckPermissions()
		{
			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted)
			{
				CheckPermission(Manifest.Permission.AccessFineLocation, PermissionsRequestAccessFineLocation);
			}

			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
			{
				CheckPermission(Manifest.Permission.AccessCoarseLocation, PermissionsRequestAccessCoarseLocation);
			}

			//if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
			//{
			//	CheckPermission(Manifest.Permission.ReadExternalStorage, PermissionsRequestAccessReadStorage);
			//}

			//if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
			//{
			//	CheckPermission(Manifest.Permission.WriteExternalStorage, PermissionsRequestAccessWriteStorage);
			//}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		private void CheckPermission(string permission, int permissionsRequestCode)
		{
			ActivityCompat.ShouldShowRequestPermissionRationale(this, permission);
			ActivityCompat.RequestPermissions(this,
											  new[]
											  {
												  permission
											  },
											  permissionsRequestCode);
		}
		#endregion
	}
}
