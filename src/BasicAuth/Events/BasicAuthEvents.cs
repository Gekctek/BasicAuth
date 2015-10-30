using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edjCase.BasicAuth.Abstractions;
using Microsoft.AspNet.Http;

namespace edjCase.BasicAuth.Events
{
	public class BasicAuthEvents : IBasicAuthEvents
	{
		/// <summary>
		/// Invoked if exceptions are thrown during request processing. The exceptions will be re-thrown after this event unless suppressed.
		/// </summary>
		public Func<BasicAuthFailedContext, Task> OnAuthenticationFailed { get; set; } = context => Task.FromResult(0);

		/// <summary>
		/// Invoked when a basic auth request is first received.
		/// </summary>
		public Func<BasicAuthDetectedContext, Task> OnRequestDetected { get; set; } = context => Task.FromResult(0);

		/// <summary>
		/// Invoked after the basic auth request has been parsed from the http message.
		/// </summary>
		public Func<BasicAuthParsedContext, Task> OnRequestParsed { get; set; } = context => Task.FromResult(0);

		/// <summary>
		/// Invoked after the basic auth request has been validated and a ClaimsIdentity has been generated.
		/// </summary>
		public Func<BasicAuthValidatedContext, Task> OnRequestValidated { get; set; } = context => Task.FromResult(0);

		/// <summary>
		/// Invoked to apply a challenge sent back to the caller.
		/// </summary>
		public Func<BasicAuthChallengeContext, Task> OnChallenge { get; set; } = context =>
		{
			context.Response.Headers.Append("WWW-Authenticate", $"Basic realm=\"{context.Options.Realm}\"");
			return Task.FromResult(0);
		};

		/// <summary>
		/// Invoked if exceptions are thrown during request processing. The exceptions will be re-thrown after this event unless suppressed.
		/// </summary>
		public virtual Task AuthenticationFailed(BasicAuthFailedContext context)
		{
			return this.OnAuthenticationFailed(context);
		}

		/// <summary>
		/// Invoked when a basic auth request is first received.
		/// </summary>
		public virtual Task RequestDetected(BasicAuthDetectedContext context)
		{
			return this.OnRequestDetected(context);
		}

		/// <summary>
		/// Invoked after the basic auth request has been parsed from the http message.
		/// </summary>
		public virtual Task RequestParsed(BasicAuthParsedContext context)
		{
			return this.OnRequestParsed(context);
		}

		/// <summary>
		/// Invoked after the basic auth request has been validated and a ClaimsIdentity has been generated.
		/// </summary>
		public virtual Task RequestValidated(BasicAuthValidatedContext context)
		{
			return this.OnRequestValidated(context);
		}

		/// <summary>
		/// Invoked to apply a challenge sent back to the caller.
		/// </summary>
		public virtual Task Challenge(BasicAuthChallengeContext context)
		{
			return this.OnChallenge(context);
		}
	}
}