using System;
using System.IO;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using Realms;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class LcViewModel : BaseViewModel
	{
		#region Data
		#region Fields
		private bool _isValid;
		private string _note = string.Empty;
        private string _name;
        private string _email;
        private string _organization;
        private string _passport;
        private string _inn;
        private string _ogrn;
        private string _director;
		private bool _permissionGranted;
		private string _phoneNumber = string.Empty;
		private readonly Realm _realm;
		private readonly AuthService _service = new AuthService();
		private string _userPictureSource = "user1.png";
		#endregion
		#endregion

		#region .ctor
		public LcViewModel(User user)
		{
			var con = RealmConfiguration.DefaultConfiguration;
			con.SchemaVersion = 11;
			_realm = Realm.GetInstance(con);

			UserPictureSource = "user1.png";
           
			PhoneNumber = user.PhoneNumber;

            if (user.UserType == UserType.Individual)
            {
                _name = user.Name;
                _email = user.Email;
                _passport = user.Passport;
            }
            else if (user.UserType == UserType.Entity)
            {
                _email = user.Email;
                _ogrn = user.Ogrn;
                _inn = user.Inn;
                _director = user.Director;
                _organization = user.Organization;
            }

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
            OpenProfile = new Command(CreateProfilePage);
        }
		#endregion

		#region Properties
		public ICommand UpdatePhotoCommand
		{
			get;
		}

        public ICommand OpenProfile 
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

        public string Passport 
        {
            get => _passport;
            set => SetProperty(ref _passport, value);
        }

        public string Director 
        {
            get => _director;
            set => SetProperty(ref _director, value);
        }

        public string OGRN 
        {
            get => _ogrn;
            set => SetProperty(ref _ogrn, value);
        }

        public string INN 
        {
            get => _inn;
            set => SetProperty(ref _inn, value);
        }

        public string Organization 
        {
            get => _organization;
            set => SetProperty(ref _organization, value);
        }

        public bool IsIndividual
            => App.User.UserType == UserType.Individual;

        public bool IsEntity
            => App.User.UserType == UserType.Entity;

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
			_permissionGranted = await CheckPermission(Permission.Storage, "Для загрузки фотографии необходимо разрешение на использование хранилища.");

			if (_permissionGranted)
			{
				UpdatePhotoCommand.CanExecute(null);
			}
		}

		private async void UpdatePhotoCommandExecute()
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				return;
			}

			var image = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
			{
				PhotoSize = PhotoSize.Medium
			});

			if (image == null)
			{
				return;
			}

			UserPictureSource = image.Path;
			try
			{
				_realm.Write(() =>
				{
					App.User.UserPictureSource = UserPictureSource;
				});

				using (var memoryStream = new MemoryStream())
				{
					image.GetStream()
						 .CopyTo(memoryStream);
					image.Dispose();
					_service.SetAvatar(memoryStream.ToArray(), App.User.UserToken);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

        private async void CreateProfilePage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new EditProfilePage()); 
        }
        #endregion
    }
}
