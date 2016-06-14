﻿using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace edjCase.BasicAuth.Events
{
	/// <summary>
	/// When a user configures the <see cref="BasicAuthMiddleware"/> to be notified after the basic
	/// auth request has failed but before ending the request
	/// </summary>
	public class BasicAuthChallengeContext : BaseBasicAuthContext
	{
		public BasicAuthChallengeContext(HttpContext context, BasicAuthOptions options) : base(context, options)
		{
		}
	}
}
