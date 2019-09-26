using Android.App;
using Android.Content;
using itmit.asb.app.Droid.Services;
using itmit.asb.app.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(YandexMoneyDroid))]
namespace itmit.asb.app.Droid.Services
{
	public class YandexMoneyDroid : IYandexCheckout
	{
		public void Buy()
		{
			MainActivity.Instance.InitUi();
		}
	}
}
