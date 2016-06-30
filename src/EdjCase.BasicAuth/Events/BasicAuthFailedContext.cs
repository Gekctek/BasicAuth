using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace EdjCase.BasicAuth.Events
{
	/// <summary>
	/// When a user configures the <see cref="BasicAuthMiddleware"/> to be notified prior to handling the exception
	/// </summary>
	public class BasicAuthFailedContext : BaseBasicAuthContext
	{
		public BasicAuthFailedContext(HttpContext context, BasicAuthOptions options) 
			: base(context, options)
		{

		}

		/// <summary>
		/// Gets or sets the <see cref="Exception"/>.
		/// </summary>
		public Exception Exception { get; internal set; }
	}
}
