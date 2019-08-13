using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Xamarin.Essentials;

namespace itmit.asb.app.ViewModels
{
	public class BidDetailViewModel : BaseViewModel
	{
		#region Data
		#region Fields
		private Bid _bid;
		private readonly IBidsService _bidService = new BidsService(App.User.UserToken);
		private string _email;
		private string _name;
		private string _note;
		private string _organization;
		private string _phoneNumber;
		private string _userPictureSource;
		#endregion
		#endregion

		#region .ctor
		public BidDetailViewModel(Bid bid)
		{
			_bid = bid;

			AcceptBidCommand = new RelayCommand(obj =>
												{
													if (obj is Bid bidParam)
													{
														Task.Run(() =>
														{
															AcceptBidCommandExecute(bidParam);
														});
													}
												},
												obj => CanExecuteAcceptBidCommand(obj));

			OpenMapCommand = new RelayCommand(obj =>
			{
				OpenMapCommandExecute();
			}, obj => true);

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

		private async void OpenMapCommandExecute()
		{
			await Map.OpenAsync(_bid.Location.Latitude, _bid.Location.Longitude, new MapLaunchOptions
			{
				Name = "Тревога",
				NavigationMode = NavigationMode.None
			});
		}
		#endregion

		#region Properties
		public ICommand AcceptBidCommand
		{
			get;
		}

		public string Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
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

		public ICommand OpenMapCommand
		{
			get;
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
		public void AcceptBidCommandExecute(Bid bid)
		{
			try
			{
				_bidService.SetBidStatusAsync(bid, BidStatus.Accepted);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}
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
				return bid.Guid != Guid.Empty;
			}

			return false;
		}
		#endregion
	}
}
