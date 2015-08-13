using System;
using System.Text;

namespace edjCase.BasicAuth
{
	/// <summary>
	/// Contains Basic auth username and password
	/// </summary>
	public struct BasicAuthCredential
	{
		/// <summary>
		/// Basic auth username
		/// </summary>
		public string Username { get; }
		/// <summary>
		/// Basic auth password
		/// </summary>
		public string Password { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username">Basic auth username</param>
		/// <param name="password">Basic auth password</param>
		public BasicAuthCredential(string username, string password)
		{
			this.Username = username;
			this.Password = password;
		}

		/// <summary>
		/// Parses the Basic auth header into <see cref="BasicAuthCredential"/>
		/// </summary>
		/// <param name="basicAuthValue">Value of the Basic auth header</param>
		/// <returns><see cref="BasicAuthCredential"/> with the username and password from Basic auth header</returns>
		public static BasicAuthCredential Parse(string basicAuthValue)
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