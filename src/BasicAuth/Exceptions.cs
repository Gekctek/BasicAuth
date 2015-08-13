using System;

namespace edjCase.BasicAuth
{
	/// <summary>
	/// Base Basic auth exception
	/// </summary>
	public abstract class BasicAuthException : Exception
	{
		protected BasicAuthException(string message) : base(message)
		{
		}

		protected BasicAuthException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	/// <summary>
	/// <see cref="BasicAuthException"/> for when the Basic auth credential is malformed or in an invalid format
	/// </summary>
	public class InvalidBasicCredentialException : BasicAuthException
	{
		public InvalidBasicCredentialException(string message) : base(message)
		{
		}

		public InvalidBasicCredentialException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	/// <summary>
	/// <see cref="BasicAuthException"/> for when the Basic auth configuration is invalid
	/// </summary>
	public class BasicAuthConfigurationException : BasicAuthException
	{
		public BasicAuthConfigurationException(string message) : base(message)
		{

		}
	}
}
