using System;
using System.Linq;
using System.Threading.Tasks;
using EdjCase.BasicAuth.Abstractions;
using EdjCase.BasicAuth.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace EdjCase.BasicAuth
{
	/// <summary>
	/// Basic auth handler that handles authentication and unauthorized requests
	/// </summary>
	internal class BasicAuthHandler : AuthenticationHandler<BasicAuthOptions>
	{
		/// <summary>
		/// Parses a http request into a Basic auth credential
		/// </summary>
		private IBasicAuthParser parser { get; }
		public BasicAuthHandler(IBasicAuthParser parser, IOptionsMonitor<BasicAuthOptions> options,
			ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
			this.parser = parser;
		}
		/// <summary>
		/// Handles authentication for Basic auth requests
		/// </summary>
		/// <returns>Task that results in an authentication ticket for credential or null for unauthorized request</returns>
		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			this.Logger?.LogDebug("Basic auth handling started...");
			bool hasBasicAuthHeader = this.parser.TryParseBasicAuthHeader(this.Request.Headers, out string basicAuthValue);
			if (!hasBasicAuthHeader)
			{
				return AuthenticateResult.Fail("No authorization header.");
			}
			try
			{
				this.Logger?.LogInformation("Basic auth request detected. Authenticating...");
				var detectedContext = new BasicAuthHandleRequestContext(this.Context, this.Scheme, this.Options);
				await this.Events?.RequestDetected(detectedContext);

				if (detectedContext.Result != null)
				{
					if (detectedContext.Result.Handled && detectedContext.Result.Ticket != null)
					{
						this.Logger?.LogInformation("Response was handled by the 'detected' event.");
						return AuthenticateResult.Success(detectedContext.Result.Ticket);
					}

					if (detectedContext.Result.Skipped)
					{
						this.Logger?.LogInformation("Authentication being skipped by the 'detected' event.");
						return AuthenticateResult.NoResult();
					}
				}

				this.Logger?.LogDebug("Attempting to parse Basic auth credential from header.");
				BasicAuthCredential credential = this.parser.ParseCredential(basicAuthValue);
				this.Logger?.LogDebug($"Successfully parsed credential with username '{credential.Username}'.");

				var authProperties = new AuthenticationProperties();
				BasicAuthInfo authInfo = new BasicAuthInfo(credential, authProperties, this.Context, this.Scheme.Name);


				var parsedContext = new BasicAuthHandleRequestContext(this.Context, this.Scheme, this.Options);
				await this.Events?.RequestParsed(parsedContext);


				if (parsedContext.Result != null)
				{
					if (parsedContext.Result.Handled && parsedContext.Result.Ticket != null)
					{
						this.Logger?.LogInformation("Response was handled by the 'request parsed' event.");
						return AuthenticateResult.Success(parsedContext.Result.Ticket);
					}

					if (parsedContext.Result.Skipped)
					{
						this.Logger?.LogInformation("Authentication being skipped by the 'request parsed' event");
						return AuthenticateResult.NoResult();
					}
				}

				if (this.Options.AuthenticateCredential == null)
				{
					throw new BasicAuthConfigurationException("AuthenticateCredential method was not set in the configuration");
				}

				this.Logger?.LogDebug("Calling configured credential authentication handler.");
				AuthenticationTicket ticket = await this.Options.AuthenticateCredential(authInfo);
				if (ticket == null)
				{
					this.Logger?.LogInformation("Basic auth handler failed to create authentication ticket.");
					var failedContext = new BasicAuthHandleRequestContext(this.Context, this.Scheme, this.Options);
					await this.Events?.AuthenticationFailed(failedContext, null);

					if (failedContext.Result != null)
					{
						if (failedContext.Result.Handled && failedContext.Result.Ticket != null)
						{
							this.Logger?.LogInformation("Failed auth was handled by configured exception handler.");
							return AuthenticateResult.Success(failedContext.Result.Ticket);
						}

						if (failedContext.Result.Skipped)
						{
							this.Logger?.LogInformation("Failed auth resulted in skipping of Basic auth processing.");
							return AuthenticateResult.NoResult();
						}
					}

					return AuthenticateResult.Fail("Failed to authenticate.");
				}
				this.Logger?.LogInformation("Basic auth handler successfully created authentication ticket.");

				var validatedContext = new BasicAuthHandleRequestContext(this.Context, this.Scheme, this.Options);
				await this.Events?.RequestValidated(validatedContext);


				if (validatedContext.Result != null)
				{
					if (validatedContext.Result.Handled && validatedContext.Result.Ticket != null)
					{
						this.Logger?.LogInformation("Validation was handled by configured event.");
						return AuthenticateResult.Success(validatedContext.Result.Ticket);
					}

					if (validatedContext.Result.Skipped)
					{
						this.Logger?.LogInformation("Validation resulted in skipping of Basic auth processing.");
						return AuthenticateResult.NoResult();
					}
				}

				return AuthenticateResult.Success(ticket);

			}
			catch (Exception ex)
			{
				this.Logger?.LogException(ex, "Error occurred when processing a Basic authentication request.");

				try
				{
					var failedContext = new BasicAuthHandleRequestContext(this.Context, this.Scheme, this.Options);

					this.Logger?.LogDebug("Calling configured exception handler.");
					await this.Events?.AuthenticationFailed(failedContext, ex);

					if (failedContext.Result != null)
					{
						if (failedContext.Result.Handled)
						{
							this.Logger?.LogInformation("Error was handled by configured exception handler.");
							return AuthenticateResult.Success(failedContext.Result.Ticket);
						}

						if (failedContext.Result.Skipped)
						{
							this.Logger?.LogInformation("Error resulted in skipping of Basic auth processing.");
							return AuthenticateResult.Success(null);
						}
					}

				}
				catch (Exception ex2)
				{
					this.Logger.LogException(ex2, "Unknown error when resolving error.");
				}
				this.Logger?.LogInformation("Error failed to be resolved.");
				return AuthenticateResult.Fail(ex);
			}
		}

		protected new IBasicAuthEvents Events
		{
			get { return (IBasicAuthEvents)base.Events; }
			set { base.Events = value; }
		}

		protected override Task InitializeEventsAsync()
		{
			this.Events = (IBasicAuthEvents)this.Options.Events ?? new BasicAuthEvents();
			return Task.CompletedTask;
		}


		protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
		{
			this.Logger?.LogInformation("Handling unauthorized Basic auth request.");

			var challengeContext = new BasicAuthHandleRequestContext(this.Context, this.Scheme, this.Options);
			await this.Events.Challenge(challengeContext);
			await base.HandleChallengeAsync(properties);
		}

		protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
		{
			this.Logger?.LogInformation("Handling unauthorized Basic auth request.");
			//TODO event?
			return base.HandleForbiddenAsync(properties);
		}
	}
}