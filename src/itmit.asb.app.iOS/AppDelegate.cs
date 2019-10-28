using System;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using itmit.asb.app.Services;
using Matcha.BackgroundService.iOS;
using UIKit;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(AuthService))]
[assembly: Dependency(typeof(BidsService))]

namespace itmit.asb.app.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// UserToken Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : FormsApplicationDelegate
	{
		private LocationCheck _showTrackingMap;

		#region Overrided
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
            ImageCircleRenderer.Init();
			Forms.Init();
			FormsMaps.Init();
			LoadApplication(new App());
			UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

			BackgroundAggregator.Init(this);

			_showTrackingMap = new LocationCheck((s, ev) =>
			{
				Console.WriteLine(((LocationCheck.LocationCheckEventArgs) ev).Allowed ? "Present Tracking Map ViewController" : "Disable Tracking Map");

				_showTrackingMap.Dispose();
			});

			return base.FinishedLaunching(app, options);
		}

		public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
		{
			// Check for new data, and display it

			// Inform system of fetch results
			completionHandler(UIBackgroundFetchResult.NewData);
		}
		#endregion
	}
}
