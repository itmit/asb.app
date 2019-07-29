﻿using System;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using itmit.asb.app.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace itmit.asb.app
{
    public partial class App : Application
	{
		private static UserToken _userToken = new UserToken();

		public static UserToken UserToken
		{
			get => _userToken;
			set
			{
				if (value.Token.Equals(string.Empty))
				{
					return;
				}
				_userToken = value;
			}
		}

		public static bool IsGuardUser
		{
			get;
			set;
		} = false;

		public App()
        {
            InitializeComponent();

			MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
