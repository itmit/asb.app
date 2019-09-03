using System;
using System.Globalization;
using itmit.asb.app.Models;
using Xamarin.Forms;

namespace itmit.asb.app.Converters
{
	/// <summary>
	/// Представляет механизм для конвертирования статуса в строку.
	/// </summary>
	public class BidStatusConverter : IValueConverter
	{
		#region Data
		#region Fields
		private readonly Color _acceptedColor = Color.Default;
		private readonly Color _pendingAcceptanceColor = Color.DarkRed;
		private readonly Color _processedAcceptanceColor = Color.Default;
		#endregion
		#endregion

		#region IValueConverter members
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type to which to convert the value.</param>
		/// <param name="parameter">A parameter to use during the conversion.</param>
		/// <param name="culture">The culture to use during the conversion.</param>
		/// <summary>
		/// Implement this method to convert <paramref name="value" /> to <paramref name="targetType" /> by using
		/// <paramref name="parameter" /> and <paramref name="culture" />.
		/// </summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is BidStatus status)
			{
				switch (status)
				{
					case BidStatus.Accepted:
						return _acceptedColor;
					case BidStatus.PendingAcceptance:
						return _pendingAcceptanceColor;
					case BidStatus.Processed:
						return _processedAcceptanceColor;
				}
			}

			throw new ArgumentOutOfRangeException(nameof(value));
		}

		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type to which to convert the value.</param>
		/// <param name="parameter">A parameter to use during the conversion.</param>
		/// <param name="culture">The culture to use during the conversion.</param>
		/// <summary>
		/// Implement this method to convert <paramref name="value" /> back from <paramref name="targetType" /> by using
		/// <paramref name="parameter" /> and <paramref name="culture" />.
		/// </summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Color status)
			{
				if (status.Equals(_acceptedColor))
				{
					return BidStatus.Accepted;
				}

				if (status.Equals(_pendingAcceptanceColor))
				{
					return BidStatus.PendingAcceptance;
				}

				if (status.Equals(_processedAcceptanceColor))
				{
					return BidStatus.Processed;
				}
			}

			throw new ArgumentOutOfRangeException(nameof(value));
		}
		#endregion
	}
}
