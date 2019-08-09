using Android.Content;
using itmit.asb.app.Controls;
using itmit.asb.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PrimLcPageTwo), typeof(FrameLcPagePrimTwoRenderer))]

namespace itmit.asb.app.Droid.Renderers
{
    public class FrameLcPagePrimTwoRenderer : VisualElementRenderer<Frame>
    {
        [System.Obsolete]
        public FrameLcPagePrimTwoRenderer(Context context) : base(context)
        {
            SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.Prim2));
        }
    }
}