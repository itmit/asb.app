using System;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	public interface ILocationService
	{
		#region Properties
		string LastError
		{
			get;
		}
		#endregion

		#region Overridable
		Task<bool> AddPointOnMapTask(Location location, UserToken token);

		Task<bool> AddPointOnMapTask(Location location, UserToken token, Guid guid);

		Task<bool> UpdateCurrentLocationTask(Location location, UserToken token);
		#endregion
	}
}
