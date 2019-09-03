using itmit.asb.app.Models;
using itmit.asb.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views.Guard
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WaitApplyBidsPage : ContentPage
	{
		#region .ctor
		public WaitApplyBidsPage()
		{
			InitializeComponent();
			var vm = new GuardMainViewModel(Navigation, false);
			vm.StartUpdateTimer();
			BindingContext = vm;
		}
		#endregion
	}
}
