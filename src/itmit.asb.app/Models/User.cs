using Newtonsoft.Json;

namespace itmit.asb.app.Models
{
	public class User
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
		}

		[JsonProperty("expires_at")]
		public string ExpiresAt
		{
			get;
			set;
		}
	}
}
