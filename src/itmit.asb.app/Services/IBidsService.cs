using System.Collections.Generic;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	public interface IBidsService
	{
		Task<IEnumerable<Bid>> GetBidsAsync(BidStatus status);

		void SetBidStatusAsync(Bid bid, BidStatus status);

		void CreateBid(Bid bid);
	}
}
