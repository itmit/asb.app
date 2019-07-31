using itmit.asb.app.Models;

namespace itmit.asb.app.ViewModels
{
	public class BidDetailViewModel : BaseViewModel
	{
		private Bid _bid;
		private string _userPictureSource;
		private string _organization;
		private string _phoneNumber;
		private string _node;
		private string _name;
		private string _email;

		public BidDetailViewModel(Bid bid)
		{
			_bid = bid;

			UserPictureSource = "user1.png";
			Organization = bid.Client.Organization;
			PhoneNumber = bid.Client.PhoneNumber;
			Node = bid.Client.Node;
			Name = bid.Client.Name;
			Email = bid.Client.Email;
			if (!string.IsNullOrEmpty(bid.Client.UserPictureSource) && bid.Client.UserPictureSource != "null")
			{
				UserPictureSource = bid.Client.UserPictureSource;
			}
		}


		public string UserPictureSource
		{
			get => _userPictureSource;
			set => SetProperty(ref _userPictureSource, value);
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

		public string Node
		{
			get => _node;
			set => SetProperty(ref _node, value);
		}

		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		public string Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
		}
	}
}
