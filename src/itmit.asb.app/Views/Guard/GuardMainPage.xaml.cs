using System;
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
		#region .ctor
		public GuardMainPage()
		{
			InitializeComponent();

			BindingContext = new GuardMainViewModel(Navigation);
		}
		#endregion

		#region Private
		private void MenuItem_OnClicked(object sender, EventArgs e)
		{
			App.Logout();
		}
		#endregion
	}
}
