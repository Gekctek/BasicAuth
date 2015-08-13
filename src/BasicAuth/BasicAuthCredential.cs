using System;
using System.Text;

namespace edjCase.BasicAuth
{
	public struct BasicAuthCredential
	{
		public string Username { get; }
		public string Password { get; }

		public BasicAuthCredential(string username, string password)
		{
			this.Username = username;
			this.Password = password;
		}

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