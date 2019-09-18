namespace itmit.asb.app.ViewModels
{
    public class ForgetPasswordModel : BaseViewModel
    {
		/// <summary>
		/// Номер телефона клиента, для восстановления пароля.
		/// </summary>
		private string _phoneNumber;

		/// <summary>
		/// Возвращает или устанавливает номер телефона для восстановления пароля.
		/// </summary>
		public string PhoneNumber
		{
			get => _phoneNumber;
			set => SetProperty(ref _phoneNumber, value);
		}
	}
}
