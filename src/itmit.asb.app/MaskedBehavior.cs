﻿using System.Collections.Generic;
using Xamarin.Forms;

namespace itmit.asb.app
{
	public class MaskedBehavior : Behavior<Entry>
	{
		#region Data
		#region Fields
		private string _mask = "";

		private IDictionary<int, char> _positions;
		#endregion
		#endregion

		#region Properties
		public string Mask
		{
			get => _mask;
			set
			{
				_mask = value;
				SetPositions();
			}
		}
		#endregion

		#region Overrided
		protected override void OnAttachedTo(Entry entry)
		{
			entry.TextChanged += OnEntryTextChanged;
			base.OnAttachedTo(entry);
		}

		protected override void OnDetachingFrom(Entry entry)
		{
			entry.TextChanged -= OnEntryTextChanged;
			base.OnDetachingFrom(entry);
		}
		#endregion

		#region Private
		private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
		{
			if (sender is Entry entry)
			{
				var text = entry.Text;

				if (string.IsNullOrWhiteSpace(text) || _positions == null)
				{
					return;
				}

				if (text.Length > _mask.Length)
				{
					entry.Text = text.Remove(text.Length - 1);
					return;
				}

				foreach (var position in _positions)
				{
					if (text.Length >= position.Key + 1)
					{
						var value = position.Value.ToString();
						if (text.Substring(position.Key, 1) != value)
						{
							text = text.Insert(position.Key, value);
						}
					}
				}

				if (entry.Text != text)
				{
					entry.Text = text;
				}
			}
		}

		private void SetPositions()
		{
			if (string.IsNullOrEmpty(Mask))
			{
				_positions = null;
				return;
			}

			var list = new Dictionary<int, char>();
			for (var i = 0; i < Mask.Length; i++)
			{
				if (Mask[i] != 'X')
				{
					list.Add(i, Mask[i]);
				}
			}

			_positions = list;
		}
		#endregion
	}
}
