using System;
using System.Threading.Tasks;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views.Guard;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class BidDetailViewModel : BaseViewModel
	{
		#region Data
		#region Fields
		private readonly Bid _bid;
		private string _email;
		private bool _isValid;
		private string _name;
		private string _note;
		private string _organization;
		private string _phoneNumber;
		private string _userPictureSource;
		private readonly INavigation _navigation;
		#endregion
		#endregion

		#region .ctor
		public BidDetailViewModel(Bid bid, INavigation navigation)
		{
			_bid = bid;
			_navigation = navigation;

			AcceptBidCommand = new RelayCommand(obj =>
												{
													if (obj is Bid bidParam)
													{
														Task.Run(() =>
														{
															AcceptBidCommandExecute(bidParam);
														});
														IsValid = false;
														Application.Current.MainPage.DisplayAlert("Внимание", "Статус тревоги успешно изменен", "OK");
													}
												},
												CanExecuteAcceptBidCommand);

			IsValid = !CanExecuteAcceptBidCommand(null);

			OpenMapCommand = new RelayCommand(obj =>
											  {
												  OpenMapCommandExecute();
											  },
											  obj => true);

			UserPictureSource = "user1.png";
			Organization = bid.Client.Organization;
			PhoneNumber = bid.Client.PhoneNumber;
			Note = bid.Client.Note;
			Name = bid.Client.Name;
			Email = bid.Client.Email;
			if (!string.IsNullOrEmpty(bid.Client.UserPictureSource) && bid.Client.UserPictureSource != "null")
			{
				UserPictureSource = bid.Client.UserPictureSource;
			}
		}
		#endregion

		#region Properties
		public ICommand AcceptBidCommand
		{
			get;
		}

		public ICommand OpenMapCommand
		{
			get;
		}

		public string Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
		}

		public bool IsValid
		{
			get => _isValid;
			set => SetProperty(ref _isValid, value);
		}

		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		public string Note
		{
			get => _note;
			set => SetProperty(ref _note, value);
		}

		public string Organization
		{
			get => _organization;
			set => SetProperty(ref _organization, value);
		}

		public string PhoneNumber
		{
			get => _phoneNumber;
			set => SetProperty(ref _phoneNumber, value);
		}

		public string UserPictureSource
		{
			get => _userPictureSource;
			set => SetProperty(ref _userPictureSource, value);
		}
		#endregion

		#region Public
		public async void AcceptBidCommandExecute(Bid bid)
		{
			IBidsService bidService = new BidsService(App.User.UserToken);
			await bidService.SetBidStatusAsync(bid, BidStatus.Accepted);
		}
		#endregion

		#region Private
		private bool CanExecuteAcceptBidCommand(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			if (obj is Bid bid)
			{
				return bid.Guid != Guid.Empty && bid.Status != BidStatus.Accepted;
			}

			return false;
		}

		private async void OpenMapCommandExecute()
		{
			await _navigation.PushAsync(new MapPage(_bid.Location));
			//await Map.OpenAsync(_bid.Location.Latitude,
			//					_bid.Location.Longitude,
			//					new MapLaunchOptions
			//					{
			//						Name = "Тревога",
			//						NavigationMode = NavigationMode.None
			//					});
		}
		#endregion
	}
}
