using Newtonsoft.Json;
using Realms;

namespace itmit.asb.app.Models
{
	public class UserToken : RealmObject
	{

		[JsonProperty("access_token")]
		public string Token
		{
			get;
			set;
		} = string.Empty;

		[JsonProperty("token_type")]
		public string TokenType
		{
			get;
			set;
		} = "Bearer";

		[JsonProperty("expires_at")]
		public string ExpiresAt
		{
			get;
			set;
		}
	}
}
