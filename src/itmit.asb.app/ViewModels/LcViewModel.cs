using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Realms;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class LcViewModel : BaseViewModel
	{
		#region Data
		#region Fields
		private string _note = string.Empty;
		private string _organization = string.Empty;
		private string _phoneNumber = string.Empty;
		private string _userPictureSource = "user1.png";
		private readonly AuthService _service = new AuthService();
		private Realm _realm = Realm.GetInstance();
		private bool _permissionGranted;
		#endregion
		#endregion

		#region .ctor
		public LcViewModel(User user)
		{

			UserPictureSource = "user1.png";
			Organization = user.Organization;
			PhoneNumber = user.PhoneNumber;
			Note = user.Note;
			if (!string.IsNullOrEmpty(user.UserPictureSource) && user.UserPictureSource != "null")
			{
				UserPictureSource = user.UserPictureSource;
			}

			CheckPermission();
			UpdatePhotoCommand = new RelayCommand(obj =>
												  {
													  UpdatePhotoCommandExecute();
												  },
												  obj => CanUpdatePhotoCommandExecute());
		}

		private bool CanUpdatePhotoCommandExecute()
		{
			if (!_permissionGranted)
			{
				CheckPermission();
			}
			return CrossMedia.IsSupported && _permissionGranted;
		}

		private async void CheckPermission()
		{
			var res = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
			_permissionGranted = res == PermissionStatus.Granted;
		}
		#endregion

		#region Properties
		public ICommand UpdatePhotoCommand
		{
			get;
		}

		public string Note
		{
			get => _note;
			set
			{
				if (value != null)
				{
					_realm.Write(() =>
									 {
										 App.User.Note = value;
									 });
					_service.SetNode(value, App.User.UserToken);
				}
				SetProperty(ref _note, value);
			}
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

		#region Private
		private async void UpdatePhotoCommandExecute()
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				return;
			}

			MediaFile image = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
			{
				PhotoSize = PhotoSize.Medium
			});

			if (image == null)
			{
				return;
			}

			UserPictureSource = image.Path;

			using (var memoryStream = new MemoryStream())
			{
				image.GetStream().CopyTo(memoryStream);
				image.Dispose();
				_service.SetAvatar(memoryStream.ToArray(), App.User.UserToken);

			}
		}
		#endregion
	}
}
