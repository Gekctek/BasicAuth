using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace EdjCase.BasicAuth.Events
{
	/// <summary>
	/// When a user configures the <see cref="BasicAuthMiddleware"/> to be notified after the basic
	/// auth request has been detected but before parsing the request
	/// </summary>
	public class BasicAuthDetectedContext : BaseBasicAuthContext
	{
		public BasicAuthDetectedContext(HttpContext context, BasicAuthOptions options) : base(context, options)
		{
		}
	}
}