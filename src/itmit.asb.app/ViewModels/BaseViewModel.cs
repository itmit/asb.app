﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace itmit.asb.app.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		#region Data
		#region Fields
		private bool _isBusy;

		private string _title = string.Empty;
		#endregion
		#endregion

		#region Properties
		public bool IsBusy
		{
			get => _isBusy;
			set => SetProperty(ref _isBusy, value);
		}

		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}
		#endregion

		#region Protected
		protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
			{
				return;
			}

			backingStore = value;
			onChanged?.Invoke();
			OnPropertyChanged(propertyName);
		}
		#endregion

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			var changed = PropertyChanged;

			changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
