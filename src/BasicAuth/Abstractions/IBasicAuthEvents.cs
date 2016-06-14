using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edjCase.BasicAuth.Events;

namespace edjCase.BasicAuth.Abstractions
{
	/// <summary>
	/// Basic auth middleware events.
	/// </summary>
	public interface IBasicAuthEvents
	{
		/// <summary>
		/// Invoked if exceptions are thrown during request processing. The exceptions will be re-thrown after this event unless suppressed.
		/// </summary>
		Task AuthenticationFailed(BasicAuthFailedContext context);

		/// <summary>
		/// Invoked when a basic auth request is first received.
		/// </summary>
		Task RequestDetected(BasicAuthDetectedContext context);

		/// <summary>
		/// Invoked after the basic auth request has been parsed from the protocol message.
		/// </summary>
		Task RequestParsed(BasicAuthParsedContext context);

		/// <summary>
		/// Invoked after the basic auth request has been validated and a ClaimsIdentity has been generated.
		/// </summary>
		Task RequestValidated(BasicAuthValidatedContext context);

		/// <summary>
		/// Invoked to apply a challenge sent back to the caller.
		/// </summary>
		Task Challenge(BasicAuthChallengeContext context);
	}
}