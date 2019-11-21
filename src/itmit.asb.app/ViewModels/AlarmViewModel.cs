using System;
using System.Diagnostics;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Plugin.Permissions.Abstractions;
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

			if (!await CheckPermission(Permission.Location, "Для отправки тревоги, необходимо разрешение на использование геоданных."))
			{
				IsBusy = false;
				return;
			}

			var guid = Guid.NewGuid();
			IBidsService service = new BidsService
			{
				Token = new UserToken()
				{
					Token = (string)App.User.UserToken.Token.Clone(),
					TokenType = (string)App.User.UserToken.TokenType.Clone()
				}
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
				else
				{
					await Application.Current.MainPage.DisplayAlert("Внимание", "Ошибка сервера.", "Ок");
				}
			}

			IsBusy = false;
		}
		#endregion
	}
}
