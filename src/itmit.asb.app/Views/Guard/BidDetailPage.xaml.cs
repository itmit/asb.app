using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using itmit.asb.app.Models;
using itmit.asb.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views.Guard
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BidDetailPage : ContentPage
	{
		public BidDetailPage(Bid bid)
		{
			InitializeComponent();

			BindingContext = new BidDetailViewModel(bid);
			AcceptBidButton.CommandParameter = bid;
		}

		private async void Button_OnClicked(object sender, EventArgs e)
		{
			//await Navigation.PushAsync(new MapPage());
		}
	}
}