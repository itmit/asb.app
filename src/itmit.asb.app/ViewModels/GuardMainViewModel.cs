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
		private readonly Timer _timer;
		private BidStatus _selectedStatus;
		#endregion
		#endregion

		#region .ctor
		public GuardMainViewModel(INavigation navigation)
		{
			_navigation = navigation;
			SelectedStatus = BidStatus.Accepted;

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
				App.Logout();
				_timer.Change(Timeout.Infinite, Timeout.Infinite);
			}, obj => true);

			// создаем таймер
			_timer = new Timer(UpdateBids, null, 0, 5000);
		}

		public ICommand ExitCommand
		{
			get;
			set;
		}
		#endregion

		#region Properties
		public BidStatus SelectedStatus
		{
			get => _selectedStatus;
            set
            {
                SetProperty(ref _selectedStatus, value);
                UpdateBids();
                
            } 
		}

		public ICommand RefreshCommand
		{
			get;
		}

		public ObservableCollection<Bid> Bids
		{
			get => _bids;
			set
			{
				SetProperty(ref _bids, value);
			}
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
		public async void UpdateBids(object obj = null)
		{
			IBidsService bidsService = new BidsService(App.User.UserToken);
			List<Bid> bidsList = (await bidsService.GetBidsAsync(SelectedStatus)).ToList();
			foreach (var bid in bidsList)
			{
				bid.UpdatedAt = new DateTime(bid.CreatedAt.Ticks + 10800);
				bid.CreatedAt = new DateTime(bid.CreatedAt.Ticks + 10800);
			}

			Bids = new ObservableCollection<Bid>(bidsList);
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
