using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using itmit.asb.app.Models;
using itmit.asb.app.ViewModels;
using Realms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views.Guard
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GuardMainPage : ContentPage
	{
		public GuardMainPage()
		{
			InitializeComponent();

			BindingContext = new GuardMainViewModel(Navigation);
		}

		private void MenuItem_OnClicked(object sender, EventArgs e)
		{
			var realm = Realm.GetInstance();
			using (var transaction = realm.BeginWrite())
			{
				realm.RemoveAll<User>();
				transaction.Commit();
			}
			Application.Current.MainPage = new LoginPage();
		}
	}
}