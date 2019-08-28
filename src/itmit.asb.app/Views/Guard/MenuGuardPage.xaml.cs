using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views.Guard
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuGuardPage : ContentPage
	{
		#region .ctor
		public MenuGuardPage()
		{
			InitializeComponent();
		}
		#endregion

		#region Private
		private void Button_Clicked(object sender, EventArgs e)
		{
			NavigateFromMenu(new ApplyBidsPage());
		}

		private void Button_Clicked_1(object sender, EventArgs e)
		{
			NavigateFromMenu(new WaitApplyBidsPage());
		}

		private async void NavigateFromMenu(Page page)
		{
			if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
			{
				var navPage = new NavigationPage(page);
				var item = new ToolbarItem
				{
					Text = "Выход"
				};

				item.SetBinding(MenuItem.CommandProperty, "ExitCommand");

				navPage.ToolbarItems.Add(item);
				masterDetailPage.Detail = navPage;

				if (Device.RuntimePlatform == Device.Android)
				{
					await Task.Delay(100);
				}

				masterDetailPage.IsPresented = false;
			}
		}
		#endregion
	}
}
