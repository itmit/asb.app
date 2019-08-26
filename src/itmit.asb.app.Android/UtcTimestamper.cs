using System;

namespace itmit.asb.app.Droid
{
	public class UtcTimestamper
	{
		#region Data
		#region Fields
		private DateTime _startTime;
		private bool _wasReset;
		#endregion
		#endregion

		#region .ctor
		public UtcTimestamper() => _startTime = DateTime.UtcNow;
		#endregion

		#region Public
		public string GetFormattedTimestamp()
		{
			var duration = DateTime.UtcNow.Subtract(_startTime);

			return _wasReset ? $"Service restarted at {_startTime} ({duration:c} ago)." : $"Service started at {_startTime} ({duration:c} ago).";
		}

		public void Restart()
		{
			_startTime = DateTime.UtcNow;
			_wasReset = true;
		}
		#endregion
	}
}
