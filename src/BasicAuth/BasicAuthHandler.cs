using System;
using System.Linq;
using System.Threading.Tasks;
using edjCase.BasicAuth.Abstractions;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;
using Microsoft.Framework.Logging;

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
		protected async override Task<AuthenticationTicket> HandleAuthenticateAsync()
		{
			this.Logger?.LogVerbose("Basic auth handling started...");
			try
			{
				string basicAuthValue;
				bool hasBasicAuthHeader = this.parser.TryParseBasicAuthHeader(this.Request.Headers, out basicAuthValue);
				if (!hasBasicAuthHeader)
				{
					return null; //TODO
				}
				try
				{
					this.Logger?.LogInformation("Basic auth request detected. Authenticating...");

					this.Logger?.LogVerbose("Attempting to parse Basic auth credential from header.");
					BasicAuthCredential credential = this.parser.ParseCredential(basicAuthValue);
					this.Logger?.LogVerbose($"Successfully parsed credential with username '{credential.Username}'.");

					AuthenticationProperties authProperties = new AuthenticationProperties();

					BasicAuthInfo authInfo = new BasicAuthInfo(credential, authProperties, this.Options);

					if (this.Options.AuthenticateCredential == null)
					{
						throw new BasicAuthConfigurationException("AuthenticateCredential method was not set in the configuration");
					}

					this.Logger?.LogVerbose("Calling configured credential authentication handler.");
					AuthenticationTicket ticket = await this.Options.AuthenticateCredential(authInfo);
					if (ticket == null)
					{
						this.Logger?.LogInformation("Basic auth handler failed to create authentication ticket.");
					}
					else
					{
						this.Logger?.LogInformation("Basic auth handler successfully created authentication ticket.");
					}
					return ticket;
				}
				catch (Exception ex)
				{
					this.Logger?.LogError("Error occurred when processing a Basic authentication request.", ex);

					var failedNotification = new AuthenticationFailedNotification<string, BasicAuthOptions>(this.Context, this.Options)
					{
						ProtocolMessage = "", //TODO
						Exception = ex
					};

					if (this.Options.OnException != null)
					{
						this.Logger?.LogVerbose("Calling configured exception handler.");
						await this.Options.OnException(failedNotification);
					}
					else
					{
						this.Logger?.LogVerbose("No configured exception handler found.");
					}

					if (failedNotification.HandledResponse)
					{
						this.Logger?.LogInformation("Error was handled by configured exception handler.");
						return failedNotification.AuthenticationTicket;
					}

					if (failedNotification.Skipped)
					{
						this.Logger?.LogInformation("Error resulted in skipping of Basic auth processing.");
						return null;
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
		protected override Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
		{
			this.Logger?.LogInformation("Handling unauthorized Basic auth request.");
			this.Response.Headers.AppendValues("WWW-Authenticate", $"Basic realm=\"{this.Options.Realm}\"");
			this.Response.StatusCode = 401; //Unauthorized
			return Task.FromResult(true);
		}
	}
}
