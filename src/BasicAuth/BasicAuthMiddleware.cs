using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.WebEncoders;

namespace edjCase.BasicAuth
{
	internal class BasicAuthMiddleware : AuthenticationMiddleware<BasicAuthOptions>
	{
		public BasicAuthMiddleware(
			RequestDelegate next, 
			IOptions<BasicAuthOptions> options, 
			ILoggerFactory loggerFactory, 
			IUrlEncoder encoder, 
			ConfigureOptions<BasicAuthOptions> configureOptions) 
			: base(next, options, loggerFactory, encoder, configureOptions)
		{
		}

		protected override AuthenticationHandler<BasicAuthOptions> CreateHandler()
		{
			return new BasicAuthHandler();
		}
	}

}
