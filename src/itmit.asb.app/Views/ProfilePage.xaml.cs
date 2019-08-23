using System;
using System.Threading.Tasks;
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
			Task.Run(() =>
			{
				App.Call("+7 (911) 447-11-83");
			});
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
			var app = Application.Current as App;

			app?.Logout();
		}
		#endregion
	}
}
