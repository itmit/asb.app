using System;
using CoreLocation;
using Foundation;

namespace itmit.asb.app.iOS
{
	public class LocationCheck : NSObject, ICLLocationManagerDelegate
	{
		public class LocationCheckEventArgs : EventArgs
		{
			public readonly bool Allowed;

			public LocationCheckEventArgs(bool allowed)
			{
				this.Allowed = allowed;
			}
		}

		private CLLocationManager _locationManager;
		private EventHandler _locationStatus;

		public LocationCheck(EventHandler locationStatus)
		{
			this._locationStatus = locationStatus;
			Initialize();
		}

		public LocationCheck(NSObjectFlag x) : base(x) { Initialize(); }

		public LocationCheck(IntPtr handle) : base(handle) { Initialize(); }

		public LocationCheck(IntPtr handle, bool alloced) : base(handle, alloced) { Initialize(); }

		private void Initialize()
		{
			_locationManager = new CLLocationManager
			{
				Delegate = this
			};
			_locationManager.RequestAlwaysAuthorization();
		}

		[Export("locationManager:didChangeAuthorizationStatus:")]
		public void AuthorizationChanged(CLLocationManager manager, CLAuthorizationStatus status)
		{
			switch (status)
			{
				case CLAuthorizationStatus.AuthorizedAlways:
				case CLAuthorizationStatus.AuthorizedWhenInUse:
					_locationStatus.Invoke(_locationManager, new LocationCheckEventArgs(true));
					break;
				case CLAuthorizationStatus.Denied:
				case CLAuthorizationStatus.Restricted:
					_locationStatus.Invoke(_locationManager, new LocationCheckEventArgs(false));
					break;
			}
		}

		protected override void Dispose(bool disposing)
		{
			_locationStatus = null;
			_locationManager.Delegate = null;
			_locationManager.Dispose();
			base.Dispose(disposing);
		}
	}
}
