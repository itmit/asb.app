using System.Security.Authentication;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	/// <summary>
	/// Представляет сервис для авторизации.
	/// </summary>
	public interface IAuthService
	{
		#region Overridable
		/// <summary>
		/// Получает данные авторизованного пользователя по токену.
		/// </summary>
		/// <param name="token">Токен для получения пользователя</param>
		/// <returns>Авторизованный пользователь.</returns>
		Task<User> GetUserByTokenAsync(UserToken token);

		/// <summary>
		/// Отправляет запрос на авторизацию, по api.
		/// </summary>
		/// <param name="login">Логин для авторизации.</param>
		/// <param name="pass">Пароль для авторизации.</param>
		/// <returns>Токен авторизованного пользователя.</returns>
		/// <exception cref="AuthenticationException">Возникает при неудачной авторизации.</exception>
		Task<UserToken> LoginAsync(string login, string pass);

		Task<UserToken> RegisterAsync(User client, string password, string cPassword);

		Task<bool> ForgotPassword(string phoneNumber);

		Task<bool> CheckCode(string phoneNumber, string code);

		Task<bool> ResetPassword(string phoneNumber, string code, string password);
		#endregion
	}
}
