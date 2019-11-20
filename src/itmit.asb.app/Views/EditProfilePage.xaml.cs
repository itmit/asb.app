using itmit.asb.app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace itmit.asb.app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        public EditProfilePage()
        {
			if (Device.RuntimePlatform == Device.iOS)
			{
				BackButton.IsEnabled = true;
				BackButton.IsVisible = true;
			}
            InitializeComponent();
            BindingContext = new EditProfileViewModel();
        }

		private void BackButton_OnClicked(object sender, EventArgs e)
		{
			Navigation.PopAsync();
		}
	}
}