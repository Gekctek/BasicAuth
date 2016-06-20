using System;
using System.Linq;
using System.Threading.Tasks;
using edjCase.BasicAuth.Abstractions;
using edjCase.BasicAuth.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Logging;

namespace edjCase.BasicAuth
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
		public BasicAuthHandler(IBasicAuthParser parser)
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
			try
			{
				string basicAuthValue;
				bool hasBasicAuthHeader = this.parser.TryParseBasicAuthHeader(this.Request.Headers, out basicAuthValue);
				if (!hasBasicAuthHeader)
				{
					return AuthenticateResult.Fail("No authorization header.");
				}
				try
				{
					this.Logger?.LogInformation("Basic auth request detected. Authenticating...");
					var detectedContext = new BasicAuthDetectedContext(this.Context, this.Options);
					await this.Options.Events.RequestDetected(detectedContext);

					if (detectedContext.HandledResponse)
					{
						this.Logger?.LogInformation("Response was handled by the 'detected' event.");
						return AuthenticateResult.Success(detectedContext.Ticket);
					}

					if (detectedContext.Skipped)
					{
						this.Logger?.LogInformation("Authentication being skipped by the 'detected' event.");
						return AuthenticateResult.Success(null);
					}

					this.Logger?.LogDebug("Attempting to parse Basic auth credential from header.");
					BasicAuthCredential credential = this.parser.ParseCredential(basicAuthValue);
					this.Logger?.LogDebug($"Successfully parsed credential with username '{credential.Username}'.");

					AuthenticationProperties authProperties = new AuthenticationProperties();

					BasicAuthInfo authInfo = new BasicAuthInfo(credential, authProperties, this.Context, this.Options.AuthenticationScheme);


					var parsedContext = new BasicAuthParsedContext(this.Context, this.Options);
					await this.Options.Events.RequestParsed(parsedContext);


					if (detectedContext.HandledResponse && detectedContext.Ticket != null)
					{
						this.Logger?.LogInformation("Response was handled by the 'request parsed' event.");
						return AuthenticateResult.Success(detectedContext.Ticket);
					}

					if (detectedContext.Skipped)
					{
						this.Logger?.LogInformation("Authentication being skipped by the 'request parsed' event");
						return AuthenticateResult.Skip();
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
						var failedContext = new BasicAuthFailedContext(this.Context, this.Options);
						await this.Options.Events.AuthenticationFailed(failedContext);

						if (failedContext.HandledResponse && failedContext.Ticket != null)
						{
							this.Logger?.LogInformation("Failed auth was handled by configured exception handler.");
							return AuthenticateResult.Success(failedContext.Ticket);
						}

						if (failedContext.Skipped)
						{
							this.Logger?.LogInformation("Failed auth resulted in skipping of Basic auth processing.");
							return AuthenticateResult.Skip();
						}

						return AuthenticateResult.Fail("Failed to authenticate.");
					}
					this.Logger?.LogInformation("Basic auth handler successfully created authentication ticket.");
					BasicAuthValidatedContext validatedContext = new BasicAuthValidatedContext(this.Context, this.Options);
					await this.Options.Events.RequestValidated(validatedContext);
					return AuthenticateResult.Success(ticket);

				}
				catch (Exception ex)
				{
					this.Logger?.LogError("Error occurred when processing a Basic authentication request.", ex);

					var failedContext = new BasicAuthFailedContext(this.Context, this.Options)
					{
						Exception = ex
					};

					this.Logger?.LogDebug("Calling configured exception handler.");
					await this.Options.Events.AuthenticationFailed(failedContext);

					if (failedContext.HandledResponse)
					{
						this.Logger?.LogInformation("Error was handled by configured exception handler.");
						return AuthenticateResult.Success(failedContext.Ticket);
					}

					if (failedContext.Skipped)
					{
						this.Logger?.LogInformation("Error resulted in skipping of Basic auth processing.");
						return AuthenticateResult.Success(null);
					}

					this.Logger?.LogInformation("Error failed to be resolved.");
					throw;
				}
			}
			finally
			{
				this.Logger?.LogInformation("Basic auth handler finished.");
			}
		}


		/// <summary>
		/// Handles unauthorized Basic auth requests
		/// </summary>
		/// <param name="context">Context of authentication challenge</param>
		/// <returns>True if response is handled</returns>
		protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
		{
			this.Logger?.LogInformation("Handling unauthorized Basic auth request.");

			var challengeContext = new BasicAuthChallengeContext(this.Context, this.Options);
			await this.Options.Events.Challenge(challengeContext);
			return false;
		}

		protected override async Task FinishResponseAsync()
		{
			//TODO? seems weird
			AuthenticateResult result = await this.HandleAuthenticateOnceAsync();
			if (!result.Succeeded)
			{
				this.Response.StatusCode = 401; //Unauthorized
			}
			await base.FinishResponseAsync();
		}

		protected override Task HandleSignOutAsync(SignOutContext context)
		{
			throw new NotSupportedException();
		}

		protected override Task HandleSignInAsync(SignInContext context)
		{
			throw new NotSupportedException();
		}
	}
}