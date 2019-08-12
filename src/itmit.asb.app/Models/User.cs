using Newtonsoft.Json;
using Realms;

namespace itmit.asb.app.Models
{
	public class User : RealmObject
	{
		#region Properties
		public string Email
		{
			get;
			set;
		}

		[JsonProperty("is_guard")]
		public bool IsGuard
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Node
		{
			get;
			set;
		}

		public string Organization
		{
			get;
			set;
		}

		[JsonProperty("phone_number")]
		public string PhoneNumber
		{
			get;
			set;
		}

		[JsonProperty("user_picture")]
		public string UserPictureSource
		{
			get;
			set;
		}

		public UserToken UserToken
		{
			get;
			set;
		}
		#endregion
	}
}
