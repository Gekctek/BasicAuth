using System;
using System.Threading.Tasks;
using edjCase.BasicAuth;
using edjCase.BasicAuth.Abstractions;
using Microsoft.AspNet.Authentication;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNet.Builder
{
	/// <summary>
	/// Extension class to add Basic auth middleware to Asp.Net pipeline
	/// </summary>
	public static class BuilderExtensions
	{
		/// <summary>
		/// Extension method to add the Basic auth middleware services to the IoC container
		/// </summary>
		/// <param name="services">IoC serivce container to register Basic auth dependencies</param>
		/// <returns>IoC service container</returns>
		public static IServiceCollection AddBasicAuth(this IServiceCollection services)
		{
			return services.AddSingleton<IBasicAuthParser, DefaultBasicAuthParser>(sp =>
			{
				ILoggerFactory loggerrFactory = sp.GetService<ILoggerFactory>();
				ILogger logger = loggerrFactory?.CreateLogger("Basic Auth Parser");
				return new DefaultBasicAuthParser(logger);
			});
		}

		/// <summary>
		/// Extension method to use the Basic auth middleware
		/// </summary>
		/// <param name="app"><see cref="IApplicationBuilder"/> that is supplied by Asp.Net</param>
		/// <param name="realm">Basic Authentication Realm</param>
		/// <param name="authenticate">Function to create an authentication ticket from Basic auth request if credentials are valid</param>
		/// <param name="onException">Optional function to handle, skip or throw exception after exception is thrown during auth</param>
		/// <param name="automaticAuthentication">True to call middleware for all requests. False to call middlware for only Basic auth requests. Defaults to false</param>
		/// <returns><see cref="IApplicationBuilder"/> that includes the Basic auth middleware</returns>
		public static IApplicationBuilder UseBasicAuth(
			this IApplicationBuilder app, 
			string realm, 
			Func<BasicAuthInfo, Task<AuthenticationTicket>> authenticate, 
			Func<AuthenticationFailedNotification<string, BasicAuthOptions>, Task> onException = null, bool automaticAuthentication = false)
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
				if (onException != null)
				{
					options.OnException = onException;
				}
				options.AutomaticAuthentication = automaticAuthentication;
			};

			var configureOptions = new ConfigureOptions<BasicAuthOptions>(configure);
			return app.UseMiddleware<BasicAuthMiddleware>(configureOptions);
		}

		/// <summary>
		/// Extension method to use the Basic auth middleware with custom configuration
		/// </summary>
		/// <param name="app"><see cref="IApplicationBuilder"/> that is supplied by Asp.Net</param>
		/// <param name="options">Action to configure the Basic auth options</param>
		/// <returns><see cref="IApplicationBuilder"/> that includes the Basic auth middleware</returns>
		public static IApplicationBuilder UseBasicAuth(this IApplicationBuilder app, Action<BasicAuthOptions> options)
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
