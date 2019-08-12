using itmit.asb.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		#region .ctor
		public LoginPage()
		{
			InitializeComponent();
			BindingContext = new LoginViewModel();
		}
		#endregion
	}
}
