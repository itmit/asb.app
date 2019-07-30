using System.Collections.ObjectModel;
using System.Threading.Tasks;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class GuardMainViewModel : BaseViewModel
	{
		private ObservableCollection<Bid> _bids;
		private readonly IBidsService _bidsService = DependencyService.Get<IBidsService>();

		public GuardMainViewModel()
		{
			Task.Run(UpdateBids);
		}

		public ObservableCollection<Bid> Bids
		{
			get => _bids;
			set => SetProperty(ref _bids, value);
		}

		public Bid SelectedBid
		{
			get;
			set;
		}

		public async void UpdateBids()
		{
			var bidsList = await _bidsService.GetBidsAsync(BidStatus.PendingAcceptance);
			Bids.Clear();
			foreach (Bid bid in bidsList)
			{
				Bids.Add(bid);
			}
		}
	}
}
