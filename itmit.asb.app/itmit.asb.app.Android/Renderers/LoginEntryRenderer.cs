using Android.Content;
using itmit.asb.app.Controls;
using itmit.asb.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(LoginEntry), typeof(LoginEntryRenderer))]
namespace itmit.asb.app.Droid.Renderers
{
    public class LoginEntryRenderer : EntryRenderer
    {
        public LoginEntryRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                // Control.Background = new ColorDrawable(Android.Graphics.Color.);
               // Control.(Color.Rgb(178, 14, 11));
            }
        }
    }
}