using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Notifications;
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
		/// Handles authentication for Basic auth requests
		/// </summary>
		/// <returns>Task that results in an authentication ticket for credential or null for unauthorized request</returns>
		protected async override Task<AuthenticationTicket> HandleAuthenticateAsync()
		{
			this.Logger?.LogVerbose("Basic auth handling started...");
			try
			{
				string[] authHeaderValues;
				bool hasAuthHeader = this.Request.Headers.TryGetValue(BasicAuthConstants.AuthHeaderName, out authHeaderValues) &&
				                     authHeaderValues.Any();

				if (!hasAuthHeader)
				{
					this.Logger?.LogVerbose("Request does not contain a Basic auth header. Skipping.");
					return null; //TODO?
				}
				string basicAuthValue = authHeaderValues.First();
				bool headerIsBasicAuth = basicAuthValue.StartsWith("Basic ");
				if (!headerIsBasicAuth)
				{
					this.Logger?.LogVerbose("Request has an authentication header but is is not the Basic auth scheme. Skipping.");
					return null; //TODO?
				}
				try
				{
					this.Logger?.LogInformation("Basic auth request detected. Authenticating...");

					this.Logger?.LogVerbose("Attempting to parse Basic auth credential from header.");
					BasicAuthCredential credential = BasicAuthCredential.Parse(basicAuthValue);
					this.Logger?.LogVerbose("Successfully.");

					AuthenticationProperties authProperties = new AuthenticationProperties();

					BasicAuthInfo authInfo = new BasicAuthInfo(credential, authProperties, this.Options);

					if (this.Options.AuthenticateCredential == null)
					{
						throw new BasicAuthConfigurationException("AuthenticateCredential method was not set in the configuration");
					}

					this.Logger?.LogVerbose("Calling configured credential authentication handler.");
					AuthenticationTicket ticket = await this.Options.AuthenticateCredential(authInfo);
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
				this.Logger?.LogVerbose("Basic auth handler finished.");
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
