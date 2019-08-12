using Xamarin.Forms;

namespace itmit.asb.app.Controls
{
	public class EditorLcPage : Editor
	{
		#region Data
		#region Static
		public static BindableProperty HasRoundedCornerProperty = BindableProperty.Create(nameof(HasRoundedCorner), typeof(bool), typeof(EditorLcPage), false);

		public static BindableProperty IsExpandableProperty = BindableProperty.Create(nameof(IsExpandable), typeof(bool), typeof(EditorLcPage), false);

		public static BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(EditorLcPage), Color.LightGray);
		public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EditorLcPage));
		#endregion
		#endregion

		#region .ctor
		public EditorLcPage() => TextChanged += OnTextChanged;
		#endregion

		#region .dtor
		~EditorLcPage()
		{
			TextChanged -= OnTextChanged;
		}
		#endregion

		#region Properties
		public bool HasRoundedCorner
		{
			get => (bool) GetValue(HasRoundedCornerProperty);
			set => SetValue(HasRoundedCornerProperty, value);
		}

		public bool IsExpandable
		{
			get => (bool) GetValue(IsExpandableProperty);
			set => SetValue(IsExpandableProperty, value);
		}

		public string Placeholder
		{
			get => (string) GetValue(PlaceholderProperty);
			set => SetValue(PlaceholderProperty, value);
		}

		public Color PlaceholderColor
		{
			get => (Color) GetValue(PlaceholderColorProperty);
			set => SetValue(PlaceholderColorProperty, value);
		}
		#endregion

		#region Private
		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (IsExpandable)
			{
				InvalidateMeasure();
			}
		}
		#endregion
	}
}
