using System;

namespace edjCase.BasicAuth
{
	public abstract class BasicAuthException : Exception
	{
		protected BasicAuthException(string message) : base(message)
		{
		}

		protected BasicAuthException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public class InvalidBasicCredentialException : BasicAuthException
	{
		public InvalidBasicCredentialException(string message) : base(message)
		{
		}

		public InvalidBasicCredentialException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
