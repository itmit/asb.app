using System.Threading.Tasks;
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
													   App.Call("+7 911 447-11-83");
												   },
												   obj => CheckNetworkAccess());
			AlarmCommand = new RelayCommand(obj =>
											{
												SendAlarm(BidType.Alert);
												
												Application.Current.MainPage.DisplayAlert("Внимание", "Тревога успешна отправлена", "OK");
												
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
			var service = new BidsService(App.User.UserToken);
			service.CreateBid(new Bid
			{
				Client = App.User,
				Location = await Location.GetCurrentGeolocationAsync(GeolocationAccuracy.Best),
				Status = BidStatus.PendingAcceptance,
				Type = type
			});
		}
		#endregion
	}
}
