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

		public string TokenType
		{
			get;
			set;
		}

		public string ExpiresAt
		{
			get;
			set;
		}
	}
}
