using System;
using Android.Content;
using itmit.asb.app.Controls;
using itmit.asb.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderAlarm), typeof(BorderAlarmRenderer))]

namespace itmit.asb.app.Droid.Renderers
{
	public class BorderAlarmRenderer : VisualElementRenderer<Frame>
	{
		#region .ctor
		[Obsolete]
		public BorderAlarmRenderer(Context context)
			: base(context)
		{
			SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.border_alarm));
		}
		#endregion
	}
}
