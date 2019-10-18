﻿using itmit.asb.app.Controls;
using itmit.asb.app.iOS.Renderers;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Color = UIKit.UIColor;

[assembly: ExportRenderer(typeof(LoginEntry), typeof(LoginEntryRenderer))]
namespace itmit.asb.app.iOS.Renderers
{
    public class LoginEntryRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.BackgroundColor = Color.FromRGB(199, 11, 9);
            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}