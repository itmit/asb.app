using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using itmit.asb.app.Controls;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace itmit.asb.app.Views.Guard
{
	public class MapPage : ContentPage
	{
		private readonly CustomMap _map;
		private readonly IBidsService _bidsService;

		public MapPage(Bid bid)
		{
			//_bidsService = DependencyService.Get<IBidsService>();

			_bidsService.Token = new UserToken
			{
				Token = (string)App.User.UserToken.Token.Clone()
			};
			

			_map = new CustomMap
			{
				IsShowingUser = true,
				HeightRequest = 100,
				HasZoomEnabled = true,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			_map.Pins.Add(new Pin
			{
				Label = "Тревога",
				Position = new Position(bid.Location.Latitude, bid.Location.Longitude)
			});

			// You can use MapSpan.FromCenterAndRadius 
			//map.MoveToRegion (MapSpan.FromCenterAndRadius (new Position (37, -122), Distance.FromMiles (0.3)));
			// or create a new MapSpan object directly
			double degrees = 360 / Math.Pow(2, 14);
			_map.ZoomLevel = new Distance(250);
			_map.MoveToRegion(new MapSpan(new Position(bid.Location.Latitude, bid.Location.Longitude), degrees, degrees));

			// create map style buttons
			var street = new Button { Text = "Street" };
			var hybrid = new Button { Text = "Hybrid" };
			var satellite = new Button { Text = "Satellite" };
			street.Clicked += HandleClicked;
			hybrid.Clicked += HandleClicked;
			satellite.Clicked += HandleClicked;
			var segments = new StackLayout
			{
				Spacing = 30,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal,
				Children = { street, hybrid, satellite }
			};

			// put the page together
			var stack = new StackLayout { Spacing = 0 };
			stack.Children.Add(_map);
			stack.Children.Add(segments);
			Content = stack;

			Timer timer = new Timer(UpdateAlertLocation, bid, 0, 10000);
		}

		private void UpdateAlertLocation(object obj)
		{
			/*if (obj is Bid bid)
			{
				Device.BeginInvokeOnMainThread(async () =>
				{
					try
					{
						bid = await _bidsService.SyncBidLocation(bid);
						var position = new Position(bid.Location.Latitude, bid.Location.Longitude);
						_map.Pins.Clear();
						_map.Pins.Add(new Pin
						{
							Label = "Тревога",
							Position = position
						});
						_map.RouteCoordinates.Add(position);
						
						_map.MoveToRegion(MapSpan.FromCenterAndRadius(
											  new Position(bid.Location.Latitude, bid.Location.Longitude),
											  _map.ZoomLevel));
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
				});
			}*/
		}

		private void HandleClicked(object sender, EventArgs e)
		{
			if (sender is Button b)
			{
				switch (b.Text)
				{
					case "Street":
						_map.MapType = MapType.Street;
						break;
					case "Hybrid":
						_map.MapType = MapType.Hybrid;
						break;
					case "Satellite":
						_map.MapType = MapType.Satellite;
						break;
				}
			}
		}

		/// <summary>
		/// In response to this forum question http://forums.xamarin.com/discussion/22493/maps-visibleregion-bounds
		/// Useful if you need to send the bounds to a web service or otherwise calculate what
		/// pins might need to be drawn inside the currently visible viewport.
		/// </summary>
		private static void CalculateBoundingCoordinates(MapSpan region)
		{
			// WARNING: I haven't tested the correctness of this exhaustively!
			var center = region.Center;
			var halfheightDegrees = region.LatitudeDegrees / 2;
			var halfwidthDegrees = region.LongitudeDegrees / 2;

			var left = center.Longitude - halfwidthDegrees;
			var right = center.Longitude + halfwidthDegrees;
			var top = center.Latitude + halfheightDegrees;
			var bottom = center.Latitude - halfheightDegrees;

			// Adjust for Internation Date Line (+/- 180 degrees longitude)
			if (left < -180)
			{
				left = 180 + (180 + left);
			}

			if (right > 180)
			{
				right = (right - 180) - 180;
			}
			// I don't wrap around north or south; I don't think the map control allows this anyway

			Debug.WriteLine("Bounding box:");
			Debug.WriteLine("                    " + top);
			Debug.WriteLine("  " + left + "                " + right);
			Debug.WriteLine("                    " + bottom);
		}
	}
}
