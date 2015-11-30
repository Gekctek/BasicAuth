using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;

namespace edjCase.BasicAuth.Events
{
	/// <summary>
	/// When a user configures the <see cref="BasicAuthMiddleware"/> to be notified after the basic
	/// auth request has been parsed but before validating the request
	/// </summary>
	public class BasicAuthParsedContext : BaseBasicAuthContext
	{
		public BasicAuthParsedContext(HttpContext context, BasicAuthOptions options) 
			: base(context, options)
		{
		}
	}
}