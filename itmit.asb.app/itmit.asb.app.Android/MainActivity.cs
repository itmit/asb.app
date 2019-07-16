using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;

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

		public int PermissionsRequestAccessCoarseLocation => 100;

		public int PermissionsRequestAccessFineLocation => 100;
	}
}