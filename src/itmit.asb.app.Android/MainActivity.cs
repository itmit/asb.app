using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location;

namespace itmit.asb.app.Droid
{
    [Activity(Label = "itmit.asb.app", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted
			    || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
			{
				RequestLocationPermission();
			}

			DisplayLocationSettingsRequest();

			LoadApplication(new App());
        }

		private void RequestLocationPermission()
		{
			if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
			{
				ActivityCompat.RequestPermissions(this,
												  new[]
												  {
													  Manifest.Permission.AccessFineLocation
												  },
												  PermissionsRequestAccessFineLocation);
			}
			else
			{
				ActivityCompat.RequestPermissions(this,
												  new[]
												  {
													  Manifest.Permission.AccessFineLocation
												  },
												  PermissionsRequestAccessFineLocation);
			}

			if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessCoarseLocation))
			{
				ActivityCompat.RequestPermissions(this,
												  new[]
												  {
													  Manifest.Permission.AccessCoarseLocation
												  },
												  PermissionsRequestAccessCoarseLocation);
			}
			else
			{
				ActivityCompat.RequestPermissions(this,
												  new[]
												  {
													  Manifest.Permission.AccessCoarseLocation
												  },
												  PermissionsRequestAccessCoarseLocation);
			}
		}

		public const int PermissionsRequestAccessCoarseLocation = 100;

		public const int PermissionsRequestAccessFineLocation = 50;

		public const int RequestCheckSettings = 1;

		private void DisplayLocationSettingsRequest()
        {
            var googleApiClient = new GoogleApiClient.Builder(this).AddApi(LocationServices.API).Build();
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
                    case LocationSettingsStatusCodes.Success:
                        {
                            //DoStuffWithLocation();
                            break;
                        }
                    case LocationSettingsStatusCodes.ResolutionRequired:
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
                            StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                            break;
                        }
                }
            });
        }
	}
}