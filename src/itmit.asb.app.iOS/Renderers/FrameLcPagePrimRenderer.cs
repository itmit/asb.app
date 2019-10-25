using itmit.asb.app.Controls;
using itmit.asb.app.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PrimLcPage), typeof(FrameLcPagePrimRenderer))]

namespace itmit.asb.app.iOS.Renderers
{
    public class FrameLcPagePrimRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            Layer.BorderColor = UIColor.Black.CGColor;
            Layer.MaskedCorners = (CoreAnimation.CACornerMask)3;
            Layer.CornerRadius = 5;
            Layer.BackgroundColor = UIColor.FromRGB(31, 31, 31).CGColor;
        }
    }
}