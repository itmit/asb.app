using itmit.asb.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views.Guard
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ApplyBidsPage : ContentPage
	{
		#region .ctor
		public ApplyBidsPage()
		{
			InitializeComponent();
			BindingContext = new GuardMainViewModel(Navigation);
		}
		#endregion
	}
}
