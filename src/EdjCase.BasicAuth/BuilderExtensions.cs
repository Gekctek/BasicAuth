using System;
using System.Threading.Tasks;
using EdjCase.BasicAuth;
using EdjCase.BasicAuth.Abstractions;
using EdjCase.BasicAuth.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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
		/// Extension method to add the Basic auth middleware with custom configuration
		/// </summary>
		/// <param name="authBuilder"><see cref="AuthenticationBuilder"/> that is supplied by Asp.Net</param>
		/// <param name="options">Action to configure the Basic auth options</param>
		/// <returns><see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddBasicAuth(this AuthenticationBuilder authBuilder, Action<BasicAuthOptions> options)
		{
			authBuilder.Services.AddSingleton<IBasicAuthParser, DefaultBasicAuthParser>(sp =>
			{
				ILoggerFactory loggerrFactory = sp.GetService<ILoggerFactory>();
				ILogger logger = loggerrFactory?.CreateLogger("Basic Auth Parser");
				return new DefaultBasicAuthParser(logger);
			});

			return authBuilder.AddScheme<BasicAuthOptions, BasicAuthHandler>(BasicAuthConstants.AuthScheme, options);
		}
	}
}
