using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edjCase.BasicAuth.Abstractions;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Logging;

namespace edjCase.BasicAuth
{
	/// <summary>
	/// Default implementation of <see cref="IBasicAuthParser"/>
	/// </summary>
	public class DefaultBasicAuthParser : IBasicAuthParser
	{
		/// <summary>
		/// Logger for Basic auth parsing actions
		/// </summary>
		private ILogger logger { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger">Logger for Basic auth parsing actions</param>
		public DefaultBasicAuthParser(ILogger logger)
		{
			this.logger = logger;
		}

		/// <summary>
		/// Parses http request headers into a Basic auth credential string value
		/// </summary>
		/// <param name="headers">Headers from http request</param>
		/// <param name="basicAuthValue">The parsed value from the Basic auth header if it exists, Null if it cant be parsed</param>
		/// <returns>True if has a valid Basic auth header, otherwise False</returns>
		public bool TryParseBasicAuthHeader(IHeaderDictionary headers, out string basicAuthValue)
		{
			string[] authHeaderValues;
			bool hasAuthHeader = headers.TryGetValue(BasicAuthConstants.AuthHeaderName, out authHeaderValues) &&
								 authHeaderValues.Any();

			if (!hasAuthHeader)
			{
				this.logger?.LogVerbose("Request does not contain a Basic auth header. Skipping.");
				basicAuthValue = null;
				return false;
			}
			basicAuthValue = authHeaderValues.First();
			bool headerIsBasicAuth = basicAuthValue.StartsWith("Basic ");
			if (!headerIsBasicAuth)
			{
				this.logger?.LogVerbose("Request has an authentication header but is is not the Basic auth scheme. Skipping.");
				basicAuthValue = null;
				return false;
			}

			return true;
		}


		/// <summary>
		/// Parses the Basic auth header into <see cref="BasicAuthCredential"/>
		/// </summary>
		/// <param name="basicAuthValue">Value of the Basic auth header</param>
		/// <returns><see cref="BasicAuthCredential"/> with the username and password from Basic auth header</returns>
		public BasicAuthCredential ParseCredential(string basicAuthValue)
		{
			if (string.IsNullOrWhiteSpace(basicAuthValue))
			{
				throw new ArgumentNullException(nameof(basicAuthValue));
			}

			string credentialString;
			try
			{
				string base64Credential = basicAuthValue.Substring(6); //6 = Length of 'Basic '
				byte[] credentialBytes = Convert.FromBase64String(base64Credential);
				credentialString = Encoding.UTF8.GetString(credentialBytes);
			}
			catch (Exception ex)
			{
				throw new InvalidBasicCredentialException(basicAuthValue, ex);
			}


			var credentialSplitIndex = credentialString.IndexOf(':');
			if (credentialSplitIndex < 0)
			{
				throw new InvalidBasicCredentialException(basicAuthValue);
			}

			string username = credentialString.Substring(0, credentialSplitIndex);
			string password = credentialString.Substring(credentialSplitIndex + 1);
			return new BasicAuthCredential(username, password);
		}
	}
}
