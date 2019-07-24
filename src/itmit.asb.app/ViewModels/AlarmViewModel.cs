using itmit.asb.app.Models;
using itmit.asb.app.Services;

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
			await service.AddItemAsync(await Location.GetCurrentGeolocationAsync());
		}

        public RelayCommand AlarmCommand { get; private set; }
    }
}