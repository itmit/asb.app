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
													   SendAlarm(BidType.Call);
												   },
												   obj => CheckNetworkAccess());
			AlarmCommand = new RelayCommand(obj =>
											{
												SendAlarm(BidType.Alert);
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
		private async void SendAlarm(BidType type)
		{
			if (!App.User.IsActive)
			{
				await Application.Current.MainPage.DisplayAlert("Внимание", "Не оплачена подписка. Тревога не отправлена.", "Ок");
				return;
			}

			var guid = Guid.NewGuid();

			IBidsService service = new BidsService(App.User.UserToken);
			var res = await service.CreateBid(new Bid
			{
				Guid = guid,
				Client = App.User,
				Location = await Location.GetCurrentGeolocationAsync(GeolocationAccuracy.Best),
				Status = BidStatus.PendingAcceptance,
				Type = type
			});

			if (res)
			{
				DependencyService.Get<ILocationTrackingService>()
								 .StartService(guid);

				if (type == BidType.Call)
				{
					App.Call("+7 911 447-11-83");
				}
				else if (type == BidType.Alert)
				{
					await Application.Current.MainPage.DisplayAlert("Внимание", "Тревога отправлена", "OK");
				}
			}
			else
			{
				if (service.LastError.Equals("Client is not active"))
				{
					await Application.Current.MainPage.DisplayAlert("Внимание", "Не оплачена подписка. Тревога не отправлена.", "Ок");
				}
			}
		}
		#endregion
	}
}
