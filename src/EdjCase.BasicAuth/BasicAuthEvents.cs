using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdjCase.BasicAuth.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace EdjCase.BasicAuth.Events
{
	public class BasicAuthEvents : IBasicAuthEvents
	{
		/// <summary>
		/// Invoked if exceptions are thrown during request processing. The exceptions will be re-thrown after this event unless suppressed.
		/// </summary>
		public Func<HandleRequestContext<BasicAuthOptions>, Exception, Task> OnAuthenticationFailed { get; set; } = (context, ex) => Task.CompletedTask;

		/// <summary>
		/// Invoked when a basic auth request is first received.
		/// </summary>
		public Func<HandleRequestContext<BasicAuthOptions>, Task> OnRequestDetected { get; set; } = context => Task.CompletedTask;

		/// <summary>
		/// Invoked after the basic auth request has been parsed from the http message.
		/// </summary>
		public Func<HandleRequestContext<BasicAuthOptions>, Task> OnRequestParsed { get; set; } = context => Task.CompletedTask;

		/// <summary>
		/// Invoked after the basic auth request has been validated and a ClaimsIdentity has been generated.
		/// </summary>
		public Func<HandleRequestContext<BasicAuthOptions>, Task> OnRequestValidated { get; set; } = context => Task.CompletedTask;

		/// <summary>
		/// Invoked to apply a challenge sent back to the caller.
		/// </summary>
		public Func<HandleRequestContext<BasicAuthOptions>, Task> OnChallenge { get; set; } = context =>
		{
			context.Response.Headers.Append("WWW-Authenticate", $"Basic realm=\"{context.Options.Realm}\"");
			context.HandleResponse();
			return Task.CompletedTask;
		};

		/// <summary>
		/// Invoked if exceptions are thrown during request processing. The exceptions will be re-thrown after this event unless suppressed.
		/// </summary>
		public virtual Task AuthenticationFailed(HandleRequestContext<BasicAuthOptions> context, Exception exception)
		{
			return this.OnAuthenticationFailed(context, exception);
		}

		/// <summary>
		/// Invoked when a basic auth request is first received.
		/// </summary>
		public virtual Task RequestDetected(HandleRequestContext<BasicAuthOptions> context)
		{
			return this.OnRequestDetected(context);
		}

		/// <summary>
		/// Invoked after the basic auth request has been parsed from the http message.
		/// </summary>
		public virtual Task RequestParsed(HandleRequestContext<BasicAuthOptions> context)
		{
			return this.OnRequestParsed(context);
		}

		/// <summary>
		/// Invoked after the basic auth request has been validated and a ClaimsIdentity has been generated.
		/// </summary>
		public virtual Task RequestValidated(HandleRequestContext<BasicAuthOptions> context)
		{
			return this.OnRequestValidated(context);
		}

		/// <summary>
		/// Invoked to apply a challenge sent back to the caller.
		/// </summary>
		public virtual Task Challenge(HandleRequestContext<BasicAuthOptions> context)
		{
			return this.OnChallenge(context);
		}
	}
}