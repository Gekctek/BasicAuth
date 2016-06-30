using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace EdjCase.BasicAuth.Events
{
	/// <summary>
	/// When a user configures the <see cref="BasicAuthMiddleware"/> to be notified after the basic
	/// auth request has been validated but before ending the request
	/// </summary>
	public class BasicAuthValidatedContext : BaseBasicAuthContext
	{
		public BasicAuthValidatedContext(HttpContext context, BasicAuthOptions options)
			: base(context, options)
		{
		}
	}
}