using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
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
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(Color.Rgb(199, 11, 9));
                else
                    Control.Background.SetColorFilter(Color.Rgb(199, 11, 9), PorterDuff.Mode.SrcAtop);
            }
        }
    }
}