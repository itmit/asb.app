using System;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Location = itmit.asb.app.Models.Location;

namespace itmit.asb.app.ViewModels
{
	public class AlarmViewModel : BaseViewModel
	{
		#region .ctor
		public AlarmViewModel()
		{
			AlarmAndCallCommand = new RelayCommand(obj =>
												   {
													   var bidId = Guid.NewGuid();
													   SendAlarm(BidType.Call, bidId);
													   DependencyService.Get<ILocationTrackingService>()
																		.StartService(bidId);
													   App.Call("+7 911 447-11-83");
												   },
												   obj => CheckNetworkAccess());
			AlarmCommand = new RelayCommand(obj =>
											{
												var bidId = Guid.NewGuid();
												SendAlarm(BidType.Alert, bidId);
												DependencyService.Get<ILocationTrackingService>()
																 .StartService(bidId);
												Application.Current.MainPage.DisplayAlert("Внимание", "Тревога отправлена", "OK");
											},
											obj => CheckNetworkAccess());
		}
		#endregion

		#region Properties
		public ICommand AlarmAndCallCommand
		{
			get;
		}

		public ICommand AlarmCommand
		{
			get;
		}
		#endregion

		#region Private
		private async void SendAlarm(BidType type, Guid guid)
		{
			if (guid == Guid.Empty)
			{
				guid = Guid.NewGuid();
			}

			IBidsService service = new BidsService(App.User.UserToken);
			await service.CreateBid(new Bid
			{
				Guid = guid,
				Client = App.User,
				Location = await Location.GetCurrentGeolocationAsync(GeolocationAccuracy.Best),
				Status = BidStatus.PendingAcceptance,
				Type = type
			});
		}
		#endregion
	}
}
