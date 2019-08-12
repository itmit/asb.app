using System;
using itmit.asb.app.Models;
using itmit.asb.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views.Guard
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BidDetailPage : ContentPage
	{
		#region .ctor
		public BidDetailPage(Bid bid)
		{
			InitializeComponent();

			BindingContext = new BidDetailViewModel(bid);
			AcceptBidButton.CommandParameter = bid;
		}
		#endregion

		#region Private
		private async void Button_OnClicked(object sender, EventArgs e)
		{
			//await Navigation.PushAsync(new MapPage());
		}
		#endregion
	}
}
