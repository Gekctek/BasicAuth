using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;

namespace edjCase.BasicAuth.Events
{
	/// <summary>
	/// When a user configures the <see cref="BasicAuthMiddleware"/> to be notified after the basic
	/// auth request has failed but before ending the request
	/// </summary>
	public class BasicAuthChallengeContext : BaseControlContext<BasicAuthOptions>
	{
		public BasicAuthChallengeContext(HttpContext context, BasicAuthOptions options) 
			: base(context, options)
		{
		}
	}
}
