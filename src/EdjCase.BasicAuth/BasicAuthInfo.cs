using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;

namespace EdjCase.BasicAuth
{
	/// <summary>
	/// Container object that holds the basic auth credential, properties and options for the request
	/// </summary>
	public class BasicAuthInfo
	{
		/// <summary>
		/// Gets the Basic auth credential from the request
		/// </summary>
		public BasicAuthCredential Credential { get; }
		/// <summary>
		/// Gets the Basic auth properties from the request
		/// </summary>
		public Microsoft.AspNetCore.Authentication.AuthenticationProperties Properties { get; }
		/// <summary>
		/// Gets the http context from the request
		/// </summary>
		public HttpContext HttpContext { get; }
		/// <summary>
		/// Gets the authentication scheme from configuration
		/// </summary>
		public string Scheme { get; }

		/// <summary>
		/// Main constructor to create <see cref="BasicAuthInfo"/>
		/// </summary>
		/// <param name="credential">Basic auth credential from the request</param>
		/// <param name="httpContext">Context from the request</param>
		/// <param name="authenticationScheme">Authentication scheme from the configuration</param>
		internal BasicAuthInfo(BasicAuthCredential credential, Microsoft.AspNetCore.Authentication.AuthenticationProperties authenticationProperties, 
			HttpContext httpContext, string authenticationScheme)
		{
			this.Credential = credential;
			this.HttpContext = httpContext;
			this.Properties = authenticationProperties;
			this.Scheme = authenticationScheme;
		}
	}
}
