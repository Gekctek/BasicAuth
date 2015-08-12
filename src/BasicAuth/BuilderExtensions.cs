using System;
using System.Threading.Tasks;
using edjCase.BasicAuth;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Notifications;
using Microsoft.Framework.OptionsModel;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNet.Builder
{
	public static class BuilderExtensions
	{
		public static IApplicationBuilder UseBasicAuth(
			this IApplicationBuilder app, 
			string realm, 
			Func<BasicAuthInfo, Task<AuthenticationTicket>> authenticate, 
			Func<AuthenticationFailedNotification<string, BasicAuthOptions>, Task> onAuthFailed = null, bool automaticAuthentication = false)
		{
			if (string.IsNullOrWhiteSpace(realm))
			{
				throw new ArgumentNullException(nameof(realm));
			}
			if (authenticate == null)
			{
				throw new ArgumentNullException(nameof(authenticate));
			}
			Action<BasicAuthOptions> configure = options =>
			{
				options.Realm = realm;
				options.AuthenticateCredential = authenticate;
				if (onAuthFailed != null)
				{
					options.OnAuthFailed = onAuthFailed;
				}
				options.AutomaticAuthentication = automaticAuthentication;
			};

			var configureOptions = new ConfigureOptions<BasicAuthOptions>(configure);
			return app.UseMiddleware<BasicAuthMiddleware>(configureOptions);
		}

		public static IApplicationBuilder UseBasicAuth(
			this IApplicationBuilder app,
			Action<BasicAuthOptions> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}
			var configureOptions = new ConfigureOptions<BasicAuthOptions>(options);
			return app.UseMiddleware<BasicAuthMiddleware>(configureOptions);
		}
	}
}
