using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views.Guard;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class GuardMainViewModel : BaseViewModel
	{
		#region Data
		#region Fields
		private ObservableCollection<Bid> _bids;
		private readonly INavigation _navigation;
		private Bid _selectedBid;
		#endregion
		#endregion

		#region .ctor
		public GuardMainViewModel(INavigation navigation)
		{
			_navigation = navigation;
			Task.Run(UpdateBids);
			_bids = new ObservableCollection<Bid>();
			RefreshCommand = new RelayCommand(obj =>
											  {
												  IsBusy = true;
												  Task.Run(UpdateBids);
											  },
											  obj => !IsBusy);
		}
		#endregion

		#region Properties
		public ICommand RefreshCommand
		{
			get;
		}

		public ObservableCollection<Bid> Bids
		{
			get => _bids;
			set => SetProperty(ref _bids, value);
		}

		public Bid SelectedBid
		{
			get => _selectedBid;
			set
			{
				_selectedBid = value;
				if (value != null)
				{
					try
					{
						PushPage(new BidDetailPage(value));
					}
					catch (Exception e)
					{
						Debug.WriteLine(e);
					}

					_selectedBid = null;
				}

				OnPropertyChanged(nameof(SelectedBid));
			}
		}
		#endregion

		#region Public
		public async void UpdateBids()
		{
			IBidsService bidsService = new BidsService(App.User.UserToken);
			var bidsList = await bidsService.GetBidsAsync(BidStatus.PendingAcceptance);
			Bids.Clear();
			foreach (var bid in bidsList)
			{
				bid.UpdatedAt = new DateTime(bid.CreatedAt.Ticks + 10800);
				bid.CreatedAt = new DateTime(bid.CreatedAt.Ticks + 10800);
				Bids.Add(bid);
			}

			IsBusy = false;
		}
		#endregion

		#region Private
		private async void PushPage(Page page)
		{
			await _navigation.PushAsync(page);
		}
		#endregion
	}
}
