using System;
using System.Diagnostics;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Realms;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class LcViewModel : BaseViewModel
	{
		private string _userPictureSource = "user1.png";
		private string _organization = string.Empty;
		private string _phoneNumber = string.Empty;
		private string _node = string.Empty;

		public LcViewModel(User user)
		{
			UserPictureSource = "user1.png";
			Organization = user.Organization;
			PhoneNumber = user.PhoneNumber;
			Node = user.Node;
			if (!string.IsNullOrEmpty(user.UserPictureSource) && user.UserPictureSource != "null")
			{
				UserPictureSource = user.UserPictureSource;
			}

			UpdatePhotoCommand = new RelayCommand(obj =>
			{
				UpdatePhotoCommandExecute();
			}, obj => true);
		}

		private async void UpdatePhotoCommandExecute()
		{
			try
			{
				FileData fileData = await CrossFilePicker.Current.PickFile();

				// user canceled file picking
				if (fileData == null)
				{
					return;
				}

				var service = new AuthService();

				service.SetAvatar(fileData.DataArray, App.User.UserToken);

				Debug.WriteLine("File name chosen: " + fileData.FileName);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception choosing file: " + ex);
			}

		}

		public ICommand UpdatePhotoCommand
		{
			get;
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
