using System;
using System.Threading;
using System.Threading.Tasks;
using CoreLocation;
using itmit.asb.app.iOS.Services;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Location = Xamarin.Essentials.Location;

[assembly: Dependency(typeof(LocationServiceIos))]
namespace itmit.asb.app.iOS.Services
{
	public class LocationServiceIos : ILocationUpdatesService
	{
		public const string LocationUpdateService = "LocationUpdateServiceBackgroundTaskManager";

		/// <summary>
		/// Запускает сервис.
		/// </summary>
		public void StartService()
		{ }

		/// <summary>
		/// Останавливает сервис.
		/// </summary>
		public void StopService()
		{ }
	}
}
