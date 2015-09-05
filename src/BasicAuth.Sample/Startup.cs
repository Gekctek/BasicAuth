using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

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
			services.AddBasicAuth();
			services.AddMvc();
		}

		// Configure is called after ConfigureServices is called.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.MinimumLevel = LogLevel.Debug;
			loggerFactory.AddDebug(LogLevel.Debug);
			string realm = "Test"; //Replace with your Basic Auth realm
			app.UseBasicAuth(realm, this.AuthenticateCredential, null, true)
				.UseMvc();
		}

		private Task<AuthenticationTicket> AuthenticateCredential(BasicAuthInfo authInfo)
		{
			AuthenticationTicket ticket = null;
			if (authInfo.Credential.Username == "Test" && authInfo.Credential.Password == "Password")
			{
				ClaimsIdentity identity = new ClaimsIdentity(authInfo.Options.AuthenticationScheme);
				identity.AddClaim(new Claim(ClaimTypes.Name, "Test"));
				identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "TestId"));
				ClaimsPrincipal principal = new ClaimsPrincipal(identity);
				ticket = new AuthenticationTicket(principal, authInfo.Properties, authInfo.Options.AuthenticationScheme);
			}
			return Task.FromResult(ticket);
		}
	}
}
