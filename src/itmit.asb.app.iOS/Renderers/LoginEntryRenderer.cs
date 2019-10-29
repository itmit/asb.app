using CoreAnimation;
using CoreGraphics;
using itmit.asb.app.Controls;
using itmit.asb.app.iOS.Renderers;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LoginEntry), typeof(LoginEntryRenderer))]
namespace itmit.asb.app.iOS.Renderers
{
    public class LoginEntryRenderer : EntryRenderer
    {
        private CALayer _line;
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            _line = null;

            if (Control == null || e.NewElement == null)
                return;

            /*
            Control.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.Line;
            */
            _line = new CALayer
            {
                BorderColor = UIColor.FromRGB(239, 43, 46).CGColor,
                BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0).CGColor,
                Frame = new CGRect(0, Frame.Height / 2, Frame.Width * 2, 1f)
            };

            Control.Layer.AddSublayer(_line);
        }
    }
}