using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Xamarin.Essentials;
using Location = itmit.asb.app.Models.Location;

namespace itmit.asb.app.ViewModels
{
	public class AlarmViewModel : BaseViewModel
	{
		public AlarmViewModel()
		{
			AlarmCommand = new RelayCommand(obj => {
				SendAlarm(new BidsService(App.User.UserToken));
			}, obj => true);
		}

		private async void SendAlarm(IBidsService service)
		{
			service.CreateBid(new Bid()
			{
				Client = App.User,
				Location = await Location.GetCurrentGeolocationAsync(GeolocationAccuracy.Best),
				Status = BidStatus.PendingAcceptance
			});
		}

        public RelayCommand AlarmCommand { get; }
    }
}