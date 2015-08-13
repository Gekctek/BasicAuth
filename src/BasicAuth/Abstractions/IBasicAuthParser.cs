using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace edjCase.BasicAuth.Abstractions
{
	interface IBasicAuthParser
	{
		/// <summary>
		/// Parses http request headers into a Basic auth credential string value
		/// </summary>
		/// <param name="headers">Headers from http request</param>
		/// <param name="basicAuthValue">The parsed value from the Basic auth header if it exists, Null if it cant be parsed</param>
		/// <returns>True if has a valid Basic auth header, otherwise False</returns>
		bool TryParseBasicAuthHeader(IHeaderDictionary headers, out string basicAuthValue);

		/// <summary>
		/// Parses the Basic auth header into <see cref="BasicAuthCredential"/>
		/// </summary>
		/// <param name="basicAuthValue">Value of the Basic auth header</param>
		/// <returns><see cref="BasicAuthCredential"/> with the username and password from Basic auth header</returns>
		BasicAuthCredential ParseCredential(string basicAuthValue);
	}
}
