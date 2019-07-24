using System;
using System.Windows.Input;

namespace itmit.asb.app.ViewModels
{
	public class RelayCommand : ICommand
	{
		#region Delegates and events
		public event EventHandler CanExecuteChanged;
		#endregion

		#region Data
		#region Fields
		private readonly Func<object, bool> _canExecute;
		private readonly Action<object> _execute;
		#endregion
		#endregion

		#region .ctor
		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}
		#endregion

		#region ICommand members
		public bool CanExecute(object parameter) => _execute == null || _canExecute(parameter);

		public void Execute(object parameter)
		{
			_execute(parameter);
		}
		#endregion
	}
}
