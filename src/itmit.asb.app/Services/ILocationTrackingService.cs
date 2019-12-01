using System;

namespace itmit.asb.app.Services
{
	/// <summary>
	/// Представляет механизм для отслеживания местоположения тревоги с минимальной задержкой.
	/// </summary>
	public interface ILocationTrackingService
	{
		#region Overridable
		/// <summary>
		/// Запускает сервис для отслеживания.
		/// </summary>
		/// <param name="bidGuid">Ид тревоги.</param>
		void StartService(Guid bidGuid);


		void StopService();
		#endregion
	}
}
