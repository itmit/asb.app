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
		private readonly Realm _realm;
		private bool _permissionGranted;
		private bool _isValid;
		#endregion
		#endregion

		#region .ctor
		public LcViewModel(User user)
		{
			var con = RealmConfiguration.DefaultConfiguration;
			con.SchemaVersion = 2;
			_realm = Realm.GetInstance(con);

			UserPictureSource = "user1.png";
			Organization = user.Organization;
			PhoneNumber = user.PhoneNumber;

			SetProperty(ref _note, user.Note);

			if (!string.IsNullOrEmpty(user.UserPictureSource) && user.UserPictureSource != "null")
			{
				UserPictureSource = user.UserPictureSource;
			}

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

			IsValid = CrossMedia.IsSupported && _permissionGranted;
			return IsValid;
		}

		private async void CheckPermission()
		{
			_permissionGranted = 
				await CheckPermission(Permission.Storage, "Для загрузки фотографии необходимо разрешение на использование хранилища.");

			if (_permissionGranted)
			{
				UpdatePhotoCommand.CanExecute(null);
			}
		}
		#endregion

		#region Properties
		public ICommand UpdatePhotoCommand
		{
			get;
		}

		public bool IsValid
		{
			get => _isValid;
			set => SetProperty(ref _isValid, value);
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

			_realm.Write(() =>
			{
				App.User.UserPictureSource = UserPictureSource;
			});

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
