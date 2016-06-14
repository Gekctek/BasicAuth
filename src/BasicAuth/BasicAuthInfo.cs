using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;

namespace edjCase.BasicAuth
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
		public AuthenticationProperties Properties { get; }
		/// <summary>
		/// Gets the Basic auth options from configuration
		/// </summary>
		public BasicAuthOptions Options { get; }

		/// <summary>
		/// Main constructor to create <see cref="BasicAuthInfo"/>
		/// </summary>
		/// <param name="credential">Basic auth credential from the request</param>
		/// <param name="properties">Basic auth properties from the request</param>
		/// <param name="options">Basic auth options from configuration</param>
		internal BasicAuthInfo(BasicAuthCredential credential, AuthenticationProperties properties, BasicAuthOptions options)
		{
			this.Credential = credential;
			this.Properties = properties;
			this.Options = options;
		}
	}
}
