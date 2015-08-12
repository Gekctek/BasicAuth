using System;

namespace edjCase.BasicAuth
{
	public class BasicAuthException : Exception
	{

		public BasicAuthException(string message) : base(message)
		{
		}

		public BasicAuthException(string message, Exception innerException) : base(message, innerException)
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
