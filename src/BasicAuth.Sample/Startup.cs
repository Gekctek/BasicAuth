using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

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
			loggerFactory.AddDebug(LogLevel.Debug);

			app
				.UseBasicAuth(options =>
				{
					options.AuthenticateCredential = this.AuthenticateCredential;
					options.AutomaticAuthenticate = true;
					options.AutomaticChallenge = true;
				})
				.UseMvc();
		}

		private Task<AuthenticationTicket> AuthenticateCredential(BasicAuthInfo authInfo)
		{
			AuthenticationTicket ticket = null;
			if (authInfo.Credential.Username == "Test" && authInfo.Credential.Password == "Password")
			{
				ClaimsIdentity identity = new ClaimsIdentity(authInfo.AuthenticationScheme);
				identity.AddClaim(new Claim(ClaimTypes.Name, "Test"));
				identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "TestId"));
				ClaimsPrincipal principal = new ClaimsPrincipal(identity);
				ticket = new AuthenticationTicket(principal, authInfo.Properties, authInfo.AuthenticationScheme);
			}
			return Task.FromResult(ticket);
		}
	}

	public class Program
	{
		public static void Main(string[] args)
		{
			var host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();

			host.Run();
		}
	}
}
