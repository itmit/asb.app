﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;

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
		private string _node;
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

			UserPictureSource = "user1.png";
			Organization = bid.Client.Organization;
			PhoneNumber = bid.Client.PhoneNumber;
			Node = bid.Client.Note;
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

		public string Node
		{
			get => _node;
			set => SetProperty(ref _node, value);
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
