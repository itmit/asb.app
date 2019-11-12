
﻿using System;
 using System.Diagnostics;
 using System.Security.Authentication;
 using System.Windows.Input;
 using itmit.asb.app.Models;
 using itmit.asb.app.Services;
 using Realms;
 using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		private INavigation _navigation;
		private string _activeTo;
		private bool _isShowedIndicator;
		private bool _isShowedActivityTitle = true;

		public AboutViewModel(INavigation navigation)
		{
			Instance = this;
			
			UpdateUserCommand.Execute(null);

			_navigation = navigation;
			OpenRobokassa = new RelayCommand(obj =>
			{
				DependencyService.Get<IYandexCheckout>().Buy();

				
			}, obj => true);
		}

		public static AboutViewModel Instance
		{
			get;
			private set;
		}

		public bool IsShowedActivityTitle
		{
			get => _isShowedActivityTitle;
			set => SetProperty(ref _isShowedActivityTitle, value);
		}

		public bool IsShowedIndicator
		{
			get => _isShowedIndicator;
			set => SetProperty(ref _isShowedIndicator, value);
		}

		public string ActiveTo
		{
			get => _activeTo;
			set => SetProperty(ref _activeTo, value);
		}
		
		private readonly IAuthService _authService = DependencyService.Get<IAuthService>();

		public ICommand UpdateUserCommand => new RelayCommand(obj =>
		{
			UpdateUserExecute();
		}, obj => true);

		private async void UpdateUserExecute()
		{
			User user;
			try
			{
				var token = App.User.UserToken;
				user = await _authService.GetUserByTokenAsync(token);
			}
			catch (AuthenticationException e)
			{
				Debug.WriteLine(e);
				return;
			}

			IsShowedActivityTitle = user.ActiveFrom.Ticks > DateTime.MinValue.Ticks;
			if (IsShowedActivityTitle)
			{
				ActiveTo = user.ActiveFrom.DateTime.Add(new TimeSpan(30, 3, 0, 0))
							   .ToString("dd.MM.yyyy hh:mm");
			}
			else
			{
				ActiveTo = "Не активна";
			}
		}

		public Realm Realm
		{
			get;
			set;
		}

		public ICommand OpenRobokassa
		{
			get;
		}
	}
}
