using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Android.Util;
using Xamarin.Android.Net;

namespace itmit.asb.app.Droid
{
	public class CustomAndroidClientHandler : AndroidClientHandler
	{
		private readonly string _tag = typeof(CustomAndroidClientHandler).FullName;

		/// <summary>Creates an instance of  <see cref="T:System.Net.Http.HttpResponseMessage" /> based on the information provided in the <see cref="T:System.Net.Http.HttpRequestMessage" /> as an operation that will not block.</summary>
		/// <param name="request">The HTTP request message.</param>
		/// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was <see langword="null" />.</exception>
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			HttpResponseMessage res = new AndroidHttpResponseMessage();
			try
			{
				res = await base.SendAsync(request, cancellationToken);
			}
			catch (Java.IO.IOException e)
			{
				Log.Error(_tag, e.Message);
			}
			catch (Exception ex)
			{
				Log.Error(_tag, ex.Message);
			}

			return res;
		}
	}
}
