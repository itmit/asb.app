using System.Windows.Input;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
    public class ForgetPasswordModel : BaseViewModel
    {
		/// <summary>
		/// Номер телефона клиента, для восстановления пароля.
		/// </summary>
		private string _phoneNumber;
		private INavigation _navigation;
		private string _errorMessage = string.Empty;

		public ForgetPasswordModel(INavigation navigation)
		{
			_navigation = navigation;
			CheckPhoneNumber = new RelayCommand(obj =>
			{
				CheckPhoneNumberExecute(_phoneNumber);
			}, obj => !IsBusy);
		}

		private async void CheckPhoneNumberExecute(string phoneNumber)
		{
			IAuthService service = new AuthService();

			if (await service.ForgotPassword(phoneNumber))
			{
				await _navigation.PushAsync(new VerifyPhoneCodePage(phoneNumber));
			}
			else
			{
				ErrorMessage = "Не найден клиент с этим номером.";
			}
		}

		public string ErrorMessage
		{
			get => _errorMessage;
			set => SetProperty(ref _errorMessage, value);
		}

		/// <summary>
		/// Возвращает или устанавливает номер телефона для восстановления пароля.
		/// </summary>
		public string PhoneNumber
		{
			get => _phoneNumber;
			set => SetProperty(ref _phoneNumber, value);
		}

		public ICommand CheckPhoneNumber
		{
			get;
		}
	}
}
