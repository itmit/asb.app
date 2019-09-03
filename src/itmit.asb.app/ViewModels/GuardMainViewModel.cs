using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
		private Timer _timer;
		private App _app = Application.Current as App;
		private bool _isShowActiveBids;
		#endregion
		#endregion

		#region .ctor
		public GuardMainViewModel(INavigation navigation)
		{
			_isShowActiveBids = true;

			_navigation = navigation;

			_bids = new ObservableCollection<Bid>();
			RefreshCommand = new RelayCommand(obj =>
											  {
												  IsBusy = true;
												  Task.Run(() =>
												  {
													  UpdateBids();
												  });
											  },
											  obj => !IsBusy);

			ExitCommand = new RelayCommand(obj =>
										   {
											   var app = Application.Current as App;
											   if (app == null)
											   {
												   return;
											   }

											   app.Logout();

											   _timer?.Change(Timeout.Infinite, Timeout.Infinite);
										   },
										   obj => true);
		}

		public void StartUpdateTimer()
		{
			// создаем таймер
			_timer = new Timer(UpdateBids, null, 0, 5000);
		}

		public GuardMainViewModel(INavigation navigation, bool isShowActiveBids)
		{
			_isShowActiveBids = isShowActiveBids;

			_navigation = navigation;

			_bids = new ObservableCollection<Bid>();
			RefreshCommand = new RelayCommand(obj =>
											  {
												  IsBusy = true;
												  Task.Run(() =>
												  {
													  UpdateBids();
												  });
											  },
											  obj => !IsBusy);

			ExitCommand = new RelayCommand(obj =>
										   {
											   var app = Application.Current as App;
											   if (app == null)
											   {
												   return;
											   }

											   app.Logout();

											   _timer?.Change(Timeout.Infinite, Timeout.Infinite);
										   },
										   obj => true);
		}
		#endregion

		#region Properties
		public ICommand ExitCommand
		{
			get;
			set;
		}

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
		private async void UpdateBids(object obj = null)
		{
			if (_app == null)
			{
				_timer.Change(Timeout.Infinite, Timeout.Infinite);
				return;
			}

			if (_app.MainPage is GuardMainPage)
			{
				IBidsService bidsService = new BidsService(App.User.UserToken);
				var bidsList = (await bidsService.GetBidsAsync()).OrderBy(x => x.Status).ToList();
				var newBidsList = new List<Bid>();
				foreach (var bid in bidsList)
				{
					bid.UpdatedAt = bid.UpdatedAt.Add(new TimeSpan(0, 3, 0, 0));
					bid.CreatedAt = bid.CreatedAt.Add(new TimeSpan(0, 3, 0, 0));
					if (CheckStatusBids(bid))
					{
						newBidsList.Add(bid);
					}
				}

				Bids = new ObservableCollection<Bid>(newBidsList);
				IsBusy = false;
			}
			else
			{
				_timer.Change(Timeout.Infinite, Timeout.Infinite);

			}
		}
		#endregion

		#region Private
		private bool CheckStatusBids(Bid bid) => _isShowActiveBids && (bid.Status == BidStatus.Accepted || bid.Status == BidStatus.PendingAcceptance);

		private async void PushPage(Page page)
		{
			await _navigation.PushAsync(page);
		}
		#endregion
	}
}
