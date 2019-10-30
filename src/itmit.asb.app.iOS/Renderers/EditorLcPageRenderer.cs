using System.ComponentModel;
using Foundation;
using itmit.asb.app.Controls;
using itmit.asb.app.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EditorLcPage), typeof(EditorLcPageRenderer))]

namespace itmit.asb.app.iOS.Renderers
{
	public class EditorLcPageRenderer : EditorRenderer
	{
		#region Data
		#region Fields
		private UILabel _placeholderLabel;
		#endregion
		#endregion

		#region Public
		public void CreatePlaceholder()
		{
			if (Element is EditorLcPage element)
			{
				element.BackgroundColor = Color.FromHex("#424242");

				_placeholderLabel = new UILabel
				{
					Text = element.Placeholder,
					TextColor = element.PlaceholderColor.ToUIColor(),
					BackgroundColor = UIColor.Clear
				};
			}

			Control.AddSubview(_placeholderLabel);
		}
		#endregion

		#region Overrided
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				if (_placeholderLabel == null)
				{
					CreatePlaceholder();
				}
			}

			if (e.NewElement != null)
			{
				var customControl = (EditorLcPage) e.NewElement;

				if (Control != null)
				{
					Control.ScrollEnabled = !customControl.IsExpandable;

					Control.Layer.CornerRadius = customControl.HasRoundedCorner ? 5 : 0;
				}
			}
		}

		
		#endregion
	}
}
