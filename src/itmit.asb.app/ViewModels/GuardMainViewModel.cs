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
		private readonly bool _isShowActiveBids;
		private bool _hasWaitApplyAlerts;
		private bool _isPlaySound;
		private readonly IPlaySoundService _soundService = DependencyService.Get<IPlaySoundService>();
		private bool _hasActiveBid;
		private Bid _firstBid;
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
											   StopPlayAlertSound();
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

		public bool HasActiveBid
		{
			get => _hasActiveBid;
			set => SetProperty(ref _hasActiveBid, value);
		}

		public ICommand SelectFirstBidCommand => new RelayCommand(obj =>
		{
			try
			{
				PushPage(new BidDetailPage(FirstBid));
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}
		}, obj => true);

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
			if (_timer == null || App.User == null)
			{
				return;
			}

			IBidsService bidsService = new BidsService
			{
				Token = App.User.UserToken
			};
			var bidsList = (await bidsService.GetBidsAsync())
			               .OrderByDescending(x => x.Status)
						   .ThenByDescending(x => x.CreatedAt)
						   .ToList();

			_hasWaitApplyAlerts = false;
			var newBidsList = new List<Bid>();
			if (!string.IsNullOrEmpty(App.User.BidGuid))
			{
				Bid firstBid = bidsList.SingleOrDefault(x => x.Guid == Guid.Parse(App.User.BidGuid));
				if (firstBid != null && CheckStatusBids(firstBid))
				{
					HasActiveBid = true;
					FirstBid = firstBid;
				}
				else
				{
					HasActiveBid = false;
				}
			} 
			else
			{
				HasActiveBid = false;
			}

			foreach (var bid in bidsList)
			{
				if (string.IsNullOrEmpty(bid.Client.Name))
				{
					bid.Client.Name = bid.Client.Organization;
				}

				bid.UpdatedAt = bid.UpdatedAt.Add(new TimeSpan(0, 3, 0, 0));
				bid.CreatedAt = bid.CreatedAt.Add(new TimeSpan(0, 3, 0, 0));

				if (CheckStatusBids(bid))
				{
					newBidsList.Add(bid);
				}
			}

			Bids = new ObservableCollection<Bid>(newBidsList);

			await Task.Run(async () =>
			{
				await Task.Delay(1000);
				var user = App.User;

				if (user == null)
				{
					return;
				}

				if (!user.HasActiveBid && user.IsGuard && Bids.Any(x => x.Status == BidStatus.PendingAcceptance))
				{
					PlaybackAlertSound();
				}
				else
				{
					StopPlayAlertSound();
				}
			});
			

			IsBusy = false;
		}

		public Bid FirstBid
		{
			get => _firstBid;
			set => SetProperty(ref _firstBid, value);
		}

		private void StopPlayAlertSound()
		{
			_soundService.StopAudio();
		}

		private void PlaybackAlertSound()
		{
			if (!_isPlaySound)
			{
				_isPlaySound = true;
				_soundService.PlayAudio("alarm.mp3", true);
			}
		}
		#endregion

		#region Private
		private bool CheckStatusBids(Bid bid) => _isShowActiveBids ^ (bid.Status == BidStatus.Processed);

		private async void PushPage(Page page)
		{
			await _navigation.PushAsync(page);
		}
		#endregion
	}
}
