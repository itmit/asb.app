using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps.Model;
using itmit.asb.app.Controls;
using itmit.asb.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace itmit.asb.app.Droid.Renderers
{
	public class CustomMapRenderer : MapRenderer
	{
		private List<Position> _routeCoordinates;

		public CustomMapRenderer(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				// Unsubscribe
			}

			if (e.NewElement != null)
			{
				var formsMap = (CustomMap)e.NewElement;
				_routeCoordinates = formsMap.RouteCoordinates;
				Control.GetMapAsync(this);
			}
		}

		private void DrawPolyLine()
		{
			var polyLineOptions = new PolylineOptions();
			polyLineOptions.InvokeColor(0x66FF0000);

			foreach (var position in _routeCoordinates)
			{
				polyLineOptions.Add(new LatLng(position.Latitude, position.Longitude));
			}
			NativeMap.Clear();
			NativeMap.AddPolyline(polyLineOptions);
		}
	}
}
