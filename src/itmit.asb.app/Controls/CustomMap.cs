using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace itmit.asb.app.Controls
{
	public class CustomMap : Map
	{
		public static readonly BindableProperty ZoomLevelProperty =
			BindableProperty.Create(nameof(ZoomLevel), typeof(Distance), typeof(CustomMap), new Distance());
		public Distance ZoomLevel
		{
			get => (Distance)GetValue(ZoomLevelProperty);
			set => SetValue(ZoomLevelProperty, value);
		}

		public CustomMap()
		{
			PropertyChanged += (sender, e) =>
			{
				if (sender is CustomMap map && map.VisibleRegion != null)
				{
					ZoomLevel = map.VisibleRegion.Radius;
				}
			};
		}

		public List<Position> RouteCoordinates
		{
			get;
			set;
		} = new List<Position>();
	}
}
