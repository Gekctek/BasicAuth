using System;
using System.Threading.Tasks;
using edjCase.BasicAuth.Abstractions;
using edjCase.BasicAuth.Events;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;

namespace edjCase.BasicAuth
{
	/// <summary>
	/// Configuration options for using basic authentication middleware.
	/// </summary>
	public class BasicAuthOptions : AuthenticationOptions
	{
		public BasicAuthOptions()
		{
			this.AuthenticationScheme = BasicAuthConstants.AuthScheme;
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
		/// The object provided by the application to process events raised by the basic authentication middleware.
		/// </summary>
		public IBasicAuthEvents Events { get; set; } = new BasicAuthEvents();
	}
}
