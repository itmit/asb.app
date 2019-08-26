using System;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	public interface ILocationService
	{
		Task<bool> AddPointOnMapTask(Location location, UserToken token);
		Task<bool> AddPointOnMapTask(Location location, UserToken token, Guid guid);

		Task<bool> UpdateCurrentLocationTask(Location location, UserToken token);

		string LastError
		{
			get;
		}
	}
}
