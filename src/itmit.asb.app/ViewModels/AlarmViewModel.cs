using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
			if (IsBusy)
			{
				return;
			}

			IsBusy = true;

			if (Device.RuntimePlatform == Device.Android)
			{
				if (!await CheckPermission(Permission.Location, "Для отправки тревоги, необходимо разрешение на использование геоданных."))
				{
					IsBusy = false;
					return;
				}
			}
			
			var guid = Guid.NewGuid();
			var user = App.User;
			bool res = false;
			IBidsService service = new BidsService
				{
					AccessToken = (string)user.UserToken.Token.Clone(),
					TokenType = (string)user.UserToken.TokenType.Clone()
				};
			try
			{
				res = await service.CreateBid(new Bid
				{
					Guid = guid,
					Client = user,
					Location = await Location.GetCurrentGeolocationAsync(GeolocationAccuracy.Best),
					Status = BidStatus.PendingAcceptance,
					Type = type
				});
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
			}

			if (res)
			{
				if (type == BidType.Call)
				{
					App.Call("+7 911 447-11-83");
				}
				else if (type == BidType.Alert)
				{
					await Application.Current.MainPage.DisplayAlert("Внимание", "Тревога отправлена", "OK");
				}

				await Task.Run(() =>
				{
					DependencyService.Get<ILocationTrackingService>()
									 .StartService(guid);
				});
			}
			else
			{
				if (string.IsNullOrEmpty(service.LastError))
				{
					await Application.Current.MainPage.DisplayAlert("Внимание", "Ошибка сервера.", "Ок");
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Внимание", service.LastError, "Ок");
				}
			}

			IsBusy = false;
		}
		#endregion
	}
}
