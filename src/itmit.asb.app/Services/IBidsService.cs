using System.Collections.Generic;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	public interface IBidsService
	{
		#region Overridable
		void CreateBid(Bid bid);

		Task<IEnumerable<Bid>> GetBidsAsync(BidStatus status);

		Task<IEnumerable<Bid>> GetBidsAsync();

		void SetBidStatusAsync(Bid bid, BidStatus status);
		#endregion
	}
}
