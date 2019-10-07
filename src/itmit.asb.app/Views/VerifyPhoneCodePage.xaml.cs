using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using itmit.asb.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VerifyPhoneCodePage : ContentPage
	{
		public VerifyPhoneCodePage(string phoneNumber)
		{
			InitializeComponent();
			BindingContext = new VerifyPhoneCodeViewModel(phoneNumber, Navigation);
		}
	}
}