using System;
using System.Threading.Tasks;
using itmit.asb.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
		#region .ctor
		public AboutPage()
		{
			InitializeComponent();
			SizeChanged += OnSizeChanged;
			BindingContext = new AboutViewModel(Navigation);
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

		private void OnSizeChanged(object sender, EventArgs e)
		{
			if (Application.Current.MainPage.Width >= 600)
			{
				Subscribe.WidthRequest = 400;
				Subscribe.HorizontalOptions = LayoutOptions.Center;
				OpisSubscribe.WidthRequest = 400;
				OpisSubscribe.HorizontalOptions = LayoutOptions.Center;
			}
			else
			{
				Subscribe.WidthRequest = Application.Current.MainPage.Width;
				Subscribe.HorizontalOptions = LayoutOptions.Fill;
				OpisSubscribe.WidthRequest = Application.Current.MainPage.Width;
				OpisSubscribe.HorizontalOptions = LayoutOptions.Fill;
			}
		}
		#endregion
	}
}
