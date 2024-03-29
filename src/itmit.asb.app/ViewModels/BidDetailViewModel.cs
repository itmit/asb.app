﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views.Guard;
using Realms;
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
        private string _director;
        private string _inn;
        private string _ogrn;
        private string _passport;
		private string _note;
		private string _organization;
		private string _phoneNumber;
		private string _userPictureSource;
		private readonly INavigation _navigation;
		private bool _isVisible;
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
														CanExecuteAcceptBidCommand(null);
														IsValid = false;

														DependencyService.Get<ILocationTrackingService>()
																		 .StartService(Guid.NewGuid());

														Application.Current.MainPage.DisplayAlert("Внимание", "Статус тревоги успешно изменен", "OK");
													}
												},
												CanExecuteAcceptBidCommand);
			CloseBidCommand = new RelayCommand(obj =>
											   {
												   if (obj is Bid bidParam)
												   {
													   Task.Run(() =>
													   {
														   CloseBidCommandExecute(bidParam);
													   });
													   CanExecuteAcceptBidCommand(null);
													   IsValid = false;

													   CanExecuteCloseBidCommand(null);
													   Application.Current.MainPage.DisplayAlert("Внимание", "Статус тревоги успешно изменен", "OK");
												   }
											   },
											   CanExecuteCloseBidCommand);

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

            Passport = bid.Client.Passport;

            Director = bid.Client.Director;

            OGRN = bid.Client.Ogrn;

            INN = bid.Client.Inn;

            if (bid.Client.Type.Equals("Individual"))
            {
                IsIndividual = true;
            }
            else if (bid.Client.Type.Equals("Entity"))
            {
                IsEntity = true;
            }

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

		public ICommand CloseBidCommand
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

		public bool IsVisible
		{
			get => _isVisible;
			set => SetProperty(ref _isVisible, value);
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

        public string Director 
        { 
            get => _director; 
            set => SetProperty(ref _director, value); 
        }

        public string Passport 
        { 
            get => _passport; 
            set => SetProperty(ref _passport, value); 
        }

        public string INN 
        { 
            get => _inn; 
            set => SetProperty(ref _inn, value); 
        }

        public string OGRN 
        { 
            get => _ogrn; 
            set => SetProperty(ref _ogrn, value); 
        }

        public string UserPictureSource
		{
			get => _userPictureSource;
			set => SetProperty(ref _userPictureSource, value);
		}

        public bool IsIndividual 
        { 
            get;
            set; 
        }

        public bool IsEntity 
        { 
            get; 
            set; 
        }
        #endregion

        #region Public
        public async void AcceptBidCommandExecute(Bid bid)
		{
			var con = RealmConfiguration.DefaultConfiguration;
			con.SchemaVersion = 11;
			using (var realm = Realm.GetInstance(con))
			{
				var user = App.User;
				realm.Write(() =>
				{
					user.HasActiveBid = true;
					user.BidGuid = bid.Guid.ToString();
				});
			}

			IBidsService bidService = new BidsService
			{
				Token = App.User.UserToken
			};

			await bidService.SetBidStatusAsync(bid, BidStatus.Accepted);
		}

		public async void CloseBidCommandExecute(Bid bid)
		{
			var con = RealmConfiguration.DefaultConfiguration;
			con.SchemaVersion = 11;
			using (var realm = Realm.GetInstance(con))
			{
				var user = App.User;
				realm.Write(() =>
				{
					user.HasActiveBid = false;
					user.BidGuid = string.Empty;
				});
			}
			IBidsService bidService = new BidsService
			{
				Token = App.User.UserToken
			};

			await bidService.SetBidStatusAsync(bid, BidStatus.Processed);
		}
		#endregion

		#region Private
		private bool CanExecuteAcceptBidCommand(object obj)
		{
			if (obj == null || App.User == null)
			{
				return false;
			}

			if (obj is Bid bid)
			{
				return bid.Guid != Guid.Empty && bid.Status != BidStatus.Accepted && !App.User.HasActiveBid;
			}

			return false;
		}

		private bool CanExecuteCloseBidCommand(object obj)
		{
			if (obj == null || App.User == null)
			{
				IsVisible = false;
				return false;
			}

			if (!Guid.TryParse(App.User.BidGuid, out var guid))
			{
				IsVisible = false;
				return false;
			}

			if (obj is Bid bid)
			{
				IsVisible = bid.Guid != Guid.Empty && bid.Status == BidStatus.Accepted && guid == _bid.Guid;
				return IsVisible;
			}

			IsVisible = false;
			return IsVisible;
		}

		private async void OpenMapCommandExecute()
		{
			await _navigation.PushAsync(new MapPage(_bid));
		}
		#endregion
	}
}
