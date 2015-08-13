using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Notifications;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;

namespace edjCase.BasicAuth
{
	internal class BasicAuthHandler : AuthenticationHandler<BasicAuthOptions>
	{
		protected async override Task<AuthenticationTicket> HandleAuthenticateAsync()
		{
			string[] authHeaderValues;
			bool hasAuthHeader = this.Request.Headers.TryGetValue(BasicAuthConstants.AuthHeaderName, out authHeaderValues) &&
								 authHeaderValues.Any();

			if (!hasAuthHeader)
			{
				return null; //TODO?
			}
			string basicAuthValue = authHeaderValues.First();
			bool headerIsBasicAuth = basicAuthValue.StartsWith("Basic ");
			if (!headerIsBasicAuth)
			{
				return null; //TODO?
			}
			try
			{
				BasicAuthCredential credential = BasicAuthCredential.Parse(basicAuthValue);

				AuthenticationProperties authProperties = new AuthenticationProperties();

				BasicAuthInfo authInfo = new BasicAuthInfo(credential, authProperties, this.Options);
				AuthenticationTicket ticket = await this.Options.AuthenticateCredential(authInfo);
				return ticket;
			}
			catch (Exception ex)
			{
				var failedNotification = new AuthenticationFailedNotification<string, BasicAuthOptions>(this.Context, this.Options)
				{
					ProtocolMessage = "", //TODO
					Exception = ex
				};

				if (this.Options.OnException != null)
				{
					await this.Options.OnException(failedNotification);
				}

				if (failedNotification.HandledResponse)
				{
					return failedNotification.AuthenticationTicket;
				}

				if (failedNotification.Skipped)
				{
					return null;
				}

				throw;
			}
		}

		protected override Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
		{
			this.Response.Headers.AppendValues("WWW-Authenticate", $"Basic realm=\"{this.Options.Realm}\"");
			this.Response.StatusCode = 401; //Unauthorized
			return Task.FromResult(true);
		}
	}
}
