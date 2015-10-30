using edjCase.BasicAuth.Abstractions;
using edjCase.BasicAuth.Events;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.WebEncoders;

namespace edjCase.BasicAuth
{
	/// <summary>
	/// Basic auth authentication middleware
	/// </summary>
	internal class BasicAuthMiddleware : AuthenticationMiddleware<BasicAuthOptions>
	{
		private IBasicAuthParser parser { get; }
		public BasicAuthMiddleware(
			RequestDelegate next, 
			BasicAuthOptions options, 
			ILoggerFactory loggerFactory, 
			IUrlEncoder encoder,
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
