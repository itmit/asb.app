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
    public partial class WaitApplyBidsPage : ContentPage
    {
        public WaitApplyBidsPage()
        {
            InitializeComponent();
			BindingContext = new GuardMainViewModel(Navigation, BidStatus.PendingAcceptance);
		}
    }
}