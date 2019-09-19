using System.Windows.Input;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class ChangePasswordViewModel : BaseViewModel
	{
		private string _changePassword;
		private string _changeConfirmPassword;
		private string _errorMessage;

		public ChangePasswordViewModel(string phoneNumber, string code)
		{
			VerifyPassword = new RelayCommand(obj =>
			{
				ResetPasswordExecute(phoneNumber, code, ChangePassword, ChangeConfirmPassword);
			}, obj => true);
		}

		private async void ResetPasswordExecute(string phoneNumber, string code, string changePassword, string changeConfirmPassword)
		{
			if (!changePassword.Equals(changeConfirmPassword))
			{
				ErrorMessage = "Пароли не совпадают";
				return;
			}

			IAuthService service = new AuthService();

			if (await service.ResetPassword(phoneNumber, code, changePassword))
			{
				Application.Current.MainPage = new NavigationPage(new LoginPage());
			}
			else
			{
				ErrorMessage = "Ошибка сервера";
			}
		}

		public string ErrorMessage
		{
			get => _errorMessage;
			set => SetProperty(ref _errorMessage, value);
		}

		public string ChangePassword
		{
			get => _changePassword;
			set => SetProperty(ref _changePassword, value);
		}

		public string ChangeConfirmPassword
		{
			get => _changeConfirmPassword;
			set => SetProperty(ref _changeConfirmPassword, value);
		}

		public ICommand VerifyPassword
		{
			get;
		}
	}
}
