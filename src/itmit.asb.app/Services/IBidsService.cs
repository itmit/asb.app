using System.Collections.Generic;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	public interface IBidsService
	{
		Task<IEnumerable<Bid>> GetBidsAsync(BidStatus status);

		Task<bool> SetBidStatusAsync(Bid bid, BidStatus status);
	}
}
