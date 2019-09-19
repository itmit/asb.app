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
    public partial class ForgetPasswordPage : ContentPage
    {
        public ForgetPasswordPage()
        {
            InitializeComponent();
			BindingContext = new ForgetPasswordModel(Navigation);
		}
		
        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Уведомление", "Отправлено SMS с новым паролем", "ОK");
        }
    }
}