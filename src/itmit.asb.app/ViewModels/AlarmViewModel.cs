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
		private bool _isSentOut;

		#region .ctor
		public AlarmViewModel()
		{
			AlarmAndCallCommand = new RelayCommand(obj =>
												   {
													   IsSentOut = true;
													   SendAlarm(BidType.Call);
												   },
												   obj => CheckNetworkAccess() && !IsSentOut);
			AlarmCommand = new RelayCommand(obj =>
											{
												IsSentOut = true;
												SendAlarm(BidType.Alert);
											},
											obj => CheckNetworkAccess() && !IsSentOut);
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

		public bool IsSentOut
		{
			get => _isSentOut;
			set => SetProperty(ref _isSentOut, value);
		}
		#endregion

		#region Private
		private async void SendAlarm(BidType type)
		{
			IsBusy = true;
			if (!App.User.IsActive)
			{
				await Application.Current.MainPage.DisplayAlert("Внимание", "Не оплачена подписка. Тревога не отправлена.", "Ок");
				IsBusy = false;
				IsSentOut = false;
				return;
			}

			var guid = Guid.NewGuid();

			IBidsService service = new BidsService
			{
				Token = App.User.UserToken
			};
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
					IsSentOut = false;
					await Application.Current.MainPage.DisplayAlert("Внимание", "Не оплачена подписка. Тревога не отправлена.", "Ок");
				}
			}

			IsBusy = false;
		}
		#endregion
	}
}
