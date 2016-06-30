using System;
using System.Text;

namespace EdjCase.BasicAuth
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

	}
}