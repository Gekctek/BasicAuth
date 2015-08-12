using System;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;

namespace edjCase.BasicAuth
{
	public class BasicAuthInfo
	{
		public BasicAuthCredential Credential { get; }
		public AuthenticationProperties Properties { get; }
		public BasicAuthOptions Options { get; }

		public BasicAuthInfo(BasicAuthCredential credential, AuthenticationProperties properties, BasicAuthOptions options)
		{
			this.Credential = credential;
			this.Properties = properties;
			this.Options = options;
		}
	}
}
