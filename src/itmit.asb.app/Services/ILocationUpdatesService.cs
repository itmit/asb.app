namespace itmit.asb.app.Services
{
	/// <summary>
	/// Представляет механизм для отслеживания местоположения авторизованного клиента.
	/// </summary>
	public interface ILocationUpdatesService
	{
		#region Overridable
		/// <summary>
		/// Запускает сервис.
		/// </summary>
		void StartService();

		/// <summary>
		/// Останавливает сервис.
		/// </summary>
		void StopService();
		#endregion
	}
}
