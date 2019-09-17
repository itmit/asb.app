using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		private INavigation _navigation;

		public AboutViewModel(INavigation navigation)
		{
			_navigation = navigation;
			OpenRobokassa = new RelayCommand(obj =>
			{
				OpenRobokassaExecute();
			}, obj => true);
		}

		private async void OpenRobokassaExecute()
		{
			var view = new WebView
			{
				Margin = 10,
				Source = "https://auth.robokassa.ru/Merchant/Index.aspx?MerchantLogin=demo&OutSum=11.00&InvId=&Description=%D0%9E%D0%BF%D0%BB%D0%B0%D1%82%D0%B0%20%D0%B7%D0%B0%D0%BA%D0%B0%D0%B7%D0%B0%20%D0%B2%20%D0%A2%D0%B5%D1%81%D1%82%D0%BE%D0%B2%D0%BE%D0%BC%20%D0%BC%D0%B0%D0%B3%D0%B0%D0%B7%D0%B8%D0%BD%D0%B5%20ROBOKASSA&shp_Item=1&Culture=ru&Encoding=utf-8&Receipt=%7B%22sno%22%3A%22osn%22%2C%22items%22%3A%5B%7B%22name%22%3A%22%D0%A2%D0%B5%D1%85%D0%BD%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F%20%D0%B4%D0%BE%D0%BA%D1%83%D0%BC%D0%B5%D0%BD%D1%82%D0%B0%D1%86%D0%B8%D1%8F%20%D0%BF%D0%BE%20ROBOKASSA%22%2C%22quantity%22%3A1.0%2C%22sum%22%3A6.0%2C%22tax%22%3A%22vat18%22%7D%2C%7B%22name%22%3A%22%D0%A2%D0%B5%D1%85%D0%BD%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F%20%D0%B4%D0%BE%D0%BA%D1%83%D0%BC%D0%B5%D0%BD%D1%82%D0%B0%D1%86%D0%B8%D1%8F%20%D0%BF%D0%BE%20Robo.market%22%2C%22quantity%22%3A1.0%2C%22sum%22%3A5.0%2C%22tax%22%3A%22vat18%22%7D%5D%7D&SignatureValue=3925b771e47d405cbcbb492daa936824"
			};
			var page = new ContentPage()
			{
				Title = "ROBOKASSA",
				Content = view
			};
			await _navigation.PushAsync(page);
		}

		public ICommand OpenRobokassa
		{
			get;
		}
	}
}
