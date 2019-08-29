using System;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	public interface ILocationService
	{
		#region Properties
		/// <summary>
		/// Возвращает последнюю ошибку, которая вернулась из API.
		/// </summary>
		string LastError
		{
			get;
		}
		#endregion

		#region Overridable
		/// <summary>
		/// Создает через API точку на карте.
		/// </summary>
		/// <param name="location">Местоположение.</param>
		/// <param name="token">Токен пользователя для авторизации.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		Task<bool> AddPointOnMapTask(Location location, UserToken token);

		/// <summary>
		/// Создает через API точку на карте, и привязывает его к тревоге.
		/// </summary>
		/// <param name="location">Местоположение.</param>
		/// <param name="token">Токен пользователя для авторизации.</param>
		/// <param name="guid">Ид тревоги, местоположение которой необходимо изменить.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		Task<bool> AddPointOnMapTask(Location location, UserToken token, Guid guid);

		/// <summary>
		/// Обновляет текущее местоположение клиента.
		/// </summary>
		/// <param name="location">Местоположение.</param>
		/// <param name="token">Токен пользователя, чье местоположение необходимо обнвоить.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		Task<bool> UpdateCurrentLocationTask(Location location, UserToken token);
		#endregion
	}
}
