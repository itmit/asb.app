namespace itmit.asb.app.Models
{
	/// <summary>
	/// Представляет возможные статусы заявки.
	/// </summary>
	public enum BidStatus
	{
		/// <summary>
		/// Заявка принята охранником.
		/// </summary>
		Accepted,

		/// <summary>
		/// Заявка ожидает принятия охранником.
		/// </summary>
		PendingAcceptance
	}
}
