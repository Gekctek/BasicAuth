using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Notifications;

namespace edjCase.BasicAuth
{
	public class BasicAuthOptions : AuthenticationOptions
	{
		public BasicAuthOptions()
		{
			this.AuthenticationScheme = "Basic";
			this.Realm = "localhost";
			this.AutomaticAuthentication = true;
		}

		public string Realm { get; set; }

		public Func<BasicAuthInfo, Task<AuthenticationTicket>> AuthenticateCredential { get; set; }
		public Func<AuthenticationFailedNotification<string, BasicAuthOptions>, Task> OnException { get; set; }
	}
}
