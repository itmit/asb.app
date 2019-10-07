using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;

namespace itmit.asb.app.Droid
{
	class LocationListener : Java.Lang.Object, ILocationListener
	{
		/// <param name="location">The new location, as a Location object.</param>
		/// <summary>Called when the location has changed.</summary>
		/// <remarks>
		///                <para tool="javadoc-to-mdoc">Called when the location has changed.
		/// </para>
		///                <para tool="javadoc-to-mdoc"> There are no restrictions on the use of the supplied Location object.</para>
		///                <para tool="javadoc-to-mdoc">
		///                    <format type="text/html">
		///                        <a href="http://developer.android.com/reference/android/location/LocationListener.html#onLocationChanged(android.location.Location)" target="_blank">[Android Documentation]</a>
		///                    </format>
		///                </para>
		///            </remarks>
		/// <since version="Added in API level 1" />
		public void OnLocationChanged(Location location)
		{
		}

		/// <param name="provider">the name of the location provider associated with this
		///  update.
		/// </param>
		/// <summary>Called when the provider is disabled by the user.</summary>
		/// <remarks>
		///                <para tool="javadoc-to-mdoc">Called when the provider is disabled by the user. If requestLocationUpdates
		/// is called on an already disabled provider, this method is called
		/// immediately.</para>
		///                <para tool="javadoc-to-mdoc">
		///                    <format type="text/html">
		///                        <a href="http://developer.android.com/reference/android/location/LocationListener.html#onProviderDisabled(java.lang.String)" target="_blank">[Android Documentation]</a>
		///                    </format>
		///                </para>
		///            </remarks>
		/// <since version="Added in API level 1" />
		public void OnProviderDisabled(string provider)
		{
		}

		/// <param name="provider">the name of the location provider associated with this
		///  update.
		/// </param>
		/// <summary>Called when the provider is enabled by the user.</summary>
		/// <remarks>
		///     <para tool="javadoc-to-mdoc">Called when the provider is enabled by the user.</para>
		///     <para tool="javadoc-to-mdoc">
		///         <format type="text/html">
		///             <a href="http://developer.android.com/reference/android/location/LocationListener.html#onProviderEnabled(java.lang.String)" target="_blank">[Android Documentation]</a>
		///         </format>
		///     </para>
		/// </remarks>
		/// <since version="Added in API level 1" />
		public void OnProviderEnabled(string provider)
		{
		}

		/// <param name="provider">the name of the location provider associated with this
		/// update.</param>
		/// <param name="status">
		///                <c>
		///                    <see cref="!:Android.Locations.LocationProvider.OUT_OF_SERVICE" />
		///                </c> if the
		/// provider is out of service, and this is not expected to change in the
		/// near future; <c><see cref="!:Android.Locations.LocationProvider.TEMPORARILY_UNAVAILABLE" /></c> if
		/// the provider is temporarily unavailable but is expected to be available
		/// shortly; and <c><see cref="!:Android.Locations.LocationProvider.AVAILABLE" /></c> if the
		/// provider is currently available.</param>
		/// <param name="extras">an optional Bundle which will contain provider specific
		/// status variables.
		/// <para tool="javadoc-to-mdoc" /> A number of common key/value pairs for the extras Bundle are listed
		/// below. Providers that use any of the keys on this list must
		/// provide the corresponding value as described below.
		/// <list type="bullet"><item><term> satellites - the number of satellites used to derive the fix
		/// </term></item></list></param>
		/// <summary>Called when the provider status changes.</summary>
		/// <remarks>
		///                <para tool="javadoc-to-mdoc">Called when the provider status changes. This method is called when
		/// a provider is unable to fetch a location or if the provider has recently
		/// become available after a period of unavailability.</para>
		///                <para tool="javadoc-to-mdoc">
		///                    <format type="text/html">
		///                        <a href="http://developer.android.com/reference/android/location/LocationListener.html#onStatusChanged(java.lang.String, int, android.os.Bundle)" target="_blank">[Android Documentation]</a>
		///                    </format>
		///                </para>
		///            </remarks>
		/// <since version="Added in API level 1" />
		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
		}
	}
}

