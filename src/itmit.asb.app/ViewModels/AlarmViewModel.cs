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
				LocationDataStore service = new LocationDataStore();
				SendAlarm(service);
			}, obj => true);
		}

		private async void SendAlarm(LocationDataStore service)
		{
			await service.AddItemAsync(await Location.GetCurrentGeolocationAsync(GeolocationAccuracy.Best));
		}

        public RelayCommand AlarmCommand { get; private set; }
    }
}