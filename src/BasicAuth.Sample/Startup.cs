using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;

namespace edjCase.BasicAuth.Sample
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
		}

		// This method gets called by a runtime.
		// Use this method to add services to the container
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
		}

		// Configure is called after ConfigureServices is called.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app
				.UseBasicAuth("Test", this.AuthenticateCredential, automaticAuthentication: false)
				.UseMvc();
		}

		private Task<AuthenticationTicket> AuthenticateCredential(BasicAuthInfo authInfo)
		{
			AuthenticationTicket ticket = null;
			if (authInfo.Credential.Username == "Test" && authInfo.Credential.Password == "Password")
			{
				ClaimsPrincipal principal = new ClaimsPrincipal();
				ticket = new AuthenticationTicket(principal, authInfo.Properties, authInfo.Options.AuthenticationScheme);
			}
			return Task.FromResult(ticket);
		}
	}
}
