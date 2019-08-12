using System;
using itmit.asb.app.Models;
using itmit.asb.app.ViewModels;
using Realms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		#region .ctor
		public ProfilePage()
		{
			InitializeComponent();
			BindingContext = new LcViewModel(App.User);
		}
		#endregion

		#region Private
		private void ImageButton_Clicked(object sender, EventArgs e)
		{
			Application.Current.MainPage = new NavigationPage(new AlarmPage());
		}

		private void ImageButton_Clicked_1(object sender, EventArgs e)
		{
			Application.Current.MainPage = new NavigationPage(new AlarmPage());
		}

		private void ImageButton_Clicked_2(object sender, EventArgs e)
		{
			Application.Current.MainPage = new NavigationPage(new ProfilePage());
		}

		private void ImageButton_Clicked_3(object sender, EventArgs e)
		{
			Application.Current.MainPage = new NavigationPage(new AboutPage());
		}

		private void ImageButton_Clicked_4(object sender, EventArgs e)
		{
			var realm = Realm.GetInstance();
			using (var transaction = realm.BeginWrite())
			{
				realm.RemoveAll<User>();
				transaction.Commit();
			}

			Application.Current.MainPage = new LoginPage();
		}
		#endregion
	}
}
