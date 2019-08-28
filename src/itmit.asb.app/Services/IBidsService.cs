using System.Collections.Generic;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	public interface IBidsService
	{
		#region Overridable
		/// <summary>
		/// Создает при помощи API новую тревогу.
		/// </summary>
		/// <param name="bid">Тревога, которую необходимо сохранить на сервере.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		Task<bool> CreateBid(Bid bid);

		/// <summary>
		/// Получает тревоги в соответствии с заданным статусом.
		/// </summary>
		/// <param name="status">Статус получаемых тревог.</param>
		/// <returns>Тревоги.</returns>
		Task<IEnumerable<Bid>> GetBidsAsync(BidStatus status);

		/// <summary>
		/// Получает все тревоги.
		/// </summary>
		/// <returns>Тревоги.</returns>
		Task<IEnumerable<Bid>> GetBidsAsync();

		/// <summary>
		/// Устанавливает новый статус у тревоги.
		/// </summary>
		/// <param name="bid">Заявка у которой необходимо установить новый статус.</param>
		/// <param name="status">Новый статус тервоги.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		Task<bool> SetBidStatusAsync(Bid bid, BidStatus status);
		#endregion
	}
}
