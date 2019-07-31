using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using itmit.asb.app.Services;
using Newtonsoft.Json;
using Realms;
using Xamarin.Auth;

namespace itmit.asb.app.Models
{
	public class User : RealmObject
	{
		public UserToken UserToken 
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

		public string Node
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

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
	}
}
