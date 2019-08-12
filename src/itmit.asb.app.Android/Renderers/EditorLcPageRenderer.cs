using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using itmit.asb.app.Controls;
using itmit.asb.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EditorLcPage), typeof(EditorLcPageRenderer))]

namespace itmit.asb.app.Droid.Renderers
{
	public class EditorLcPageRenderer : EditorRenderer
	{
		#region Data
		#region Fields
		private bool initial = true;
		private Drawable originalBackground;
		#endregion
		#endregion

		#region .ctor
		public EditorLcPageRenderer(Context context)
			: base(context)
		{
		}
		#endregion

		#region Overrided
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				if (initial)
				{
					originalBackground = Control.Background;
					initial = false;
				}
			}

			if (e.NewElement != null)
			{
				var customControl = (EditorLcPage) Element;
				if (customControl.HasRoundedCorner)
				{
					ApplyBorder();
				}

				if (!string.IsNullOrEmpty(customControl.Placeholder))
				{
					Control.Hint = customControl.Placeholder;
					Control.SetHintTextColor(customControl.PlaceholderColor.ToAndroid());
				}
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var customControl = (EditorLcPage) Element;

			if (EditorLcPage.PlaceholderProperty.PropertyName == e.PropertyName)
			{
				Control.Hint = customControl.Placeholder;
			}
			else if (EditorLcPage.PlaceholderColorProperty.PropertyName == e.PropertyName)
			{
				Control.SetHintTextColor(customControl.PlaceholderColor.ToAndroid());
			}
			else if (EditorLcPage.HasRoundedCornerProperty.PropertyName == e.PropertyName)
			{
				if (customControl.HasRoundedCorner)
				{
					ApplyBorder();
				}
				else
				{
					Control.Background = originalBackground;
				}
			}
		}
		#endregion

		#region Private
		private void ApplyBorder()
		{
			var gd = new GradientDrawable();
			gd.SetCornerRadius(10);
			gd.SetStroke(2, Color.Black.ToAndroid());
			Control.Background = gd;
		}
		#endregion
	}
}
