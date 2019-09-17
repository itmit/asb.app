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
			// регистрационная информация (логин, пароль #1)
			// registration info (login, password #1)
			string sMrchLogin = "demo";
			string sMrchPass1 = "password_1";

			// номер заказа
			// number of order
			int nInvId = 0;

			// описание заказа
			// order description
			string sDesc = "Оплата заказа в Тестовом магазине ROBOKASSA";

			// сумма заказа
			// sum of order
			string sOutSum = "11.00";

			// тип товара
			// code of goods
			string sShpItem = "1";

			// язык
			// language
			string sCulture = "ru";

			// кодировка
			// encoding
			string sEncoding = "utf-8";
			// формирование подписи
			// generate signature
			string sCrcBase = $"{sMrchLogin}:{sOutSum}:{nInvId}:{sMrchPass1}:shp_Item={sShpItem}";

			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] bSignature = md5.ComputeHash(Encoding.UTF8.GetBytes(sCrcBase));

			StringBuilder sbSignature = new StringBuilder();
			foreach (byte b in bSignature)
			{
				sbSignature.AppendFormat("{0:x2}", b);
			}

			string sCrc = sbSignature.ToString();
			var str = "https://auth.robokassa.ru/Merchant/Index.aspx?" +
					  "MerchantLogin=" +
					  sMrchLogin +
					  "&OutSum=" +
					  sOutSum +
					  "&InvId=" +
					  nInvId +
					  "&shp_Item=" +
					  sShpItem +
					  "&SignatureValue=" +
					  sCrc +
					  "&Description=" +
					  sDesc +
					  "&Culture=" +
					  sCulture +
					  "&Encoding=" +
					  sEncoding;
			var view = new WebView
			{
				Source = str,
				Margin = 10
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
