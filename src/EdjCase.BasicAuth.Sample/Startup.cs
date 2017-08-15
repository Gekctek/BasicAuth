using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using EdjCase.BasicAuth.Events;
using System;

namespace EdjCase.BasicAuth.Sample
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
			services
				.AddAuthentication(BasicAuthConstants.AuthScheme)
				.AddBasicAuth(options =>
				{
					options.AuthenticateCredential = this.AuthenticateCredential;
					options.Events = new BasicAuthEvents
					{
						OnAuthenticationFailed = this.OnAuthenticationFailed
					};
				});
			services.AddMvc();
		}

		// Configure is called after ConfigureServices is called.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddDebug(LogLevel.Debug);

			app
				.UseAuthentication()
				.UseMvc();
		}

		private Task<AuthenticationTicket> AuthenticateCredential(BasicAuthInfo authInfo)
		{
			AuthenticationTicket ticket = null;
			if (authInfo.Credential.Username == "Test" && authInfo.Credential.Password == "Password")
			{
				ClaimsIdentity identity = new ClaimsIdentity(authInfo.Scheme);
				identity.AddClaim(new Claim(ClaimTypes.Name, "Test"));
				identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "TestId"));
				ClaimsPrincipal principal = new ClaimsPrincipal(identity);
				ticket = new AuthenticationTicket(principal, authInfo.Properties, authInfo.Scheme);
			}
			return Task.FromResult(ticket);
		}

		private Task OnAuthenticationFailed(HandleRequestContext<BasicAuthOptions> context, Exception exception)
		{
			//if...(something that can be handled)...context.HandleResponse();
			//if...(should skip to next middleware)...context.SkipToNextMiddleware();
			return Task.FromResult(0);
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
