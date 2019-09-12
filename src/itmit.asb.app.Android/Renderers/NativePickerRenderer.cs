using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using itmit.asb.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(Picker), typeof(NativePickerRenderer))]
namespace itmit.asb.app.Droid.Renderers
{
    public class NativePickerRenderer : PickerRenderer
    {
        public NativePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Control.BackgroundTintList = ColorStateList.ValueOf(Color.Rgb(199, 11, 9));
                }
                else
                {
                    Control.Background.SetColorFilter(Color.Rgb(199, 11, 9), PorterDuff.Mode.SrcAtop);
                }
            }
        }
    }
}