using System;
using System.Threading.Tasks;
using EdjCase.BasicAuth.Abstractions;
using EdjCase.BasicAuth.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace EdjCase.BasicAuth
{
	/// <summary>
	/// Configuration options for using basic authentication middleware.
	/// </summary>
	public class BasicAuthOptions : AuthenticationSchemeOptions
	{		
		/// <summary>
		/// Basic authentication realm
		/// </summary>
		public string Realm { get; set; } = "localhost";

		/// <summary>
		/// Required function to check request credential and create authentication ticket
		/// </summary>
		public Func<BasicAuthInfo, Task<AuthenticationTicket>> AuthenticateCredential { get; set; }
	}
}
