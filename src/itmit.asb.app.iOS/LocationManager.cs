using System;
using CoreLocation;
using UIKit;

namespace itmit.asb.app.iOS
{
	public class LocationManager
	{
		private readonly CLLocationManager _locMgr;

		public LocationManager()
		{
			_locMgr = new CLLocationManager
			{
				PausesLocationUpdatesAutomatically = false
			};

			// iOS 8 has additional permissions requirements
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				_locMgr.RequestAlwaysAuthorization(); // works in background
				//locMgr.RequestWhenInUseAuthorization (); // only in foreground
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
			{
				_locMgr.AllowsBackgroundLocationUpdates = true;
			}
		}

		public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

		public CLLocationManager LocMgr => _locMgr;

		public void StartLocationUpdates()
		{
			if (CLLocationManager.LocationServicesEnabled)
			{
				//set the desired accuracy, in meters
				LocMgr.DesiredAccuracy = 25;
				LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
				{
					// fire our custom Location Updated event
					LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
				};
				LocMgr.StartUpdatingLocation();
			}
		}
	}
}
