using System;
using System.Threading.Tasks;
using edjCase.BasicAuth;
using edjCase.BasicAuth.Abstractions;
using edjCase.BasicAuth.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
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
			BasicAuthOptions basicAuthOptions = new BasicAuthOptions();
			options(basicAuthOptions);

			return app.UseMiddleware<BasicAuthMiddleware>(Options.Create(basicAuthOptions));
		}
	}
}
