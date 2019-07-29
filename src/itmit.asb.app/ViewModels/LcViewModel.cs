using itmit.asb.app.Models;

namespace itmit.asb.app.ViewModels
{
	public class LcViewModel : BaseViewModel
	{
		private string _userPictureSource = "";
		private string _organization = "";
		private string _phoneNumber = "";
		private string _node = "";
		private User _user;

		public LcViewModel()
		{
			UserPictureSource = "user1.png";
		}

		public async void LoadUserAsync(UserToken token)
		{
			_user = await User.GetUserByTokenAsync(token);

			if (_user.UserPictureSource != string.Empty 
			    || _user.UserPictureSource != null
				|| _user.UserPictureSource != "null"

				)
			{
				UserPictureSource = _user.UserPictureSource;
			}
			Organization = _user.Organization;
			PhoneNumber = _user.PhoneNumber;
			Node = _user.Node;
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
	}
}
