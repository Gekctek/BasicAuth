using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Notifications;

namespace edjCase.BasicAuth
{
	/// <summary>
	/// Configuration options for using basic authentication middleware.
	/// </summary>
	public class BasicAuthOptions : AuthenticationOptions
	{
		public BasicAuthOptions()
		{
			this.AuthenticationScheme = "Basic";
		}
		/// <summary>
		/// Basic authentication realm
		/// </summary>
		public string Realm { get; set; } = "localhost";
		/// <summary>
		/// Required function to check request credential and create authentication ticket
		/// </summary>
		public Func<BasicAuthInfo, Task<AuthenticationTicket>> AuthenticateCredential { get; set; }
		/// <summary>
		/// Optional function to handle, skip or throw exception that was caught during the basic auth processing
		/// </summary>
		public Func<AuthenticationFailedNotification<string, BasicAuthOptions>, Task> OnException { get; set; }
	}
}
