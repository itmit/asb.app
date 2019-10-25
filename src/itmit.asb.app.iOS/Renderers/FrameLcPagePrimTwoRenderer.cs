using itmit.asb.app.Controls;
using itmit.asb.app.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PrimLcPageTwo), typeof(FrameLcPagePrimRenderer))]

namespace itmit.asb.app.iOS.Renderers
{
    public class FrameLcPagePrimTwoRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            Layer.BorderColor = UIColor.Black.CGColor;
            Layer.MaskedCorners = (CoreAnimation.CACornerMask)12;
            Layer.CornerRadius = 5;
            Layer.BackgroundColor = UIColor.FromRGB(42, 42, 42).CGColor;
        }
    }
}