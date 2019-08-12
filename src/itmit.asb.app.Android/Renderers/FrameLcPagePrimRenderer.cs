using System;
using Android.Content;
using itmit.asb.app.Controls;
using itmit.asb.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PrimLcPage), typeof(FrameLcPagePrimRenderer))]

namespace itmit.asb.app.Droid.Renderers
{
	public class FrameLcPagePrimRenderer : VisualElementRenderer<Frame>
	{
		#region .ctor
		[Obsolete]
		public FrameLcPagePrimRenderer(Context context)
			: base(context)
		{
			SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.Prim));
		}
		#endregion
	}
}
