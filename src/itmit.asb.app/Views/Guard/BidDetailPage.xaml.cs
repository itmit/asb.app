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

			BindingContext = new BidDetailViewModel(bid, Navigation);
			AcceptBidButton.CommandParameter = bid;
			SizeChanged += OnSizeChanged;
		}
		#endregion

		#region Private
		private void OnSizeChanged(object sender, EventArgs e)
		{
			if (Application.Current.MainPage.Width >= 600)
			{
				Name.FontSize = 40;
				Organization.FontSize = 40;
				PhoneNumber.FontSize = 40;
				Email.FontSize = 40;
				Note.FontSize = 40;
			}
			else
			{
				Name.FontSize = 20;
				Organization.FontSize = 20;
				PhoneNumber.FontSize = 20;
				Email.FontSize = 20;
				Note.FontSize = 20;
			}
		}
		#endregion
	}
}
