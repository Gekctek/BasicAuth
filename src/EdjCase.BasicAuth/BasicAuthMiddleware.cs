using EdjCase.BasicAuth.Abstractions;
using EdjCase.BasicAuth.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;

namespace EdjCase.BasicAuth
{
	/// <summary>
	/// Basic auth authentication middleware
	/// </summary>
	internal class BasicAuthMiddleware : AuthenticationMiddleware<BasicAuthOptions>
	{
		private IBasicAuthParser parser { get; }
		public BasicAuthMiddleware(
			RequestDelegate next,
			IOptions<BasicAuthOptions> options,
			ILoggerFactory loggerFactory,
			UrlEncoder encoder,
			IBasicAuthParser parser)
			: base(next, options, loggerFactory, encoder)
		{
			this.parser = parser;

			if (this.Options.Events == null)
			{
				this.Options.Events = new BasicAuthEvents();
			}
		}

		/// <summary>
		/// Creates handler for the Basic auth request
		/// </summary>
		/// <returns>Basic auth request authentication handler</returns>
		protected override AuthenticationHandler<BasicAuthOptions> CreateHandler()
		{
			return new BasicAuthHandler(this.parser);
		}
	}

}
