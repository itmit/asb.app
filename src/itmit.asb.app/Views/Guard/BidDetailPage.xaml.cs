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
			CloseBidButton.CommandParameter = bid;
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
				PhoneNumberIn.FontSize = 40;
                PhoneNumberEn.FontSize = 40;
				EmailIn.FontSize = 40;
                EmailEn.FontSize = 40;
                Director.FontSize = 40;
                INN.FontSize = 40;
                Ogrn.FontSize = 40;
				Note.FontSize = 40;
                Passport.FontSize = 40;
                
			}
			else
			{
				Name.FontSize = 20;
				Organization.FontSize = 20;
				PhoneNumberIn.FontSize = 20;
                PhoneNumberEn.FontSize = 20;
				EmailIn.FontSize = 20;
                EmailEn.FontSize = 20;
                Director.FontSize = 20;
                INN.FontSize = 20;
                Ogrn.FontSize = 20;
                Note.FontSize = 20;
                Passport.FontSize = 20;
			}
		}
		#endregion
	}
}
