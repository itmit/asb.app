using System.Windows.Input;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class VerifyPhoneCodeViewModel : BaseViewModel
	{
		private string _phoneCode;
		private INavigation _navigation;

		public VerifyPhoneCodeViewModel(string phoneNumber, INavigation navigation)
		{
			_navigation = navigation;
			
			VerifyPhoneCode = new RelayCommand(obj =>
			{
				VerifyPhoneCodeExecute(phoneNumber, PhoneCode);
			}, obj => true);
		}

		private async void VerifyPhoneCodeExecute(string phoneNumber, string code)
		{
			AuthService service = new AuthService();

			if (await service.CheckCode(phoneNumber, code))
			{
				await _navigation.PushAsync(new ChangePasswordPage());
			}
		}

		/// <summary>
		/// Возвращает или устанавливает код проверки.
		/// </summary>
		public string PhoneCode
		{
			get => _phoneCode;
			set => SetProperty(ref _phoneCode, value);
		}

		/// <summary>
		/// Возвращает или устанавливает команду для проверки кода.
		/// </summary>
		public ICommand VerifyPhoneCode
		{
			get;
		}
	}
}
