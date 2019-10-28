using CoreLocation;

namespace itmit.asb.app.iOS
{
	public class LocationUpdatedEventArgs
	{
		public LocationUpdatedEventArgs(CLLocation location) => Location = location;

		public CLLocation Location
		{
			get;
		}
	}
}
