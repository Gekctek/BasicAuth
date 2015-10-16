﻿using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;
using System;

namespace edjCase.BasicAuth
{
	/// <summary>
	/// When a user configures the <see cref="BasicAuthMiddleware"/> to be notified prior to handling the exception
	/// </summary>
	public class BasicAuthFailedContext : BaseControlContext<BasicAuthOptions>
	{
		public BasicAuthFailedContext(HttpContext context, BasicAuthOptions options) : base(context, options)
		{

		}

		/// <summary>
		/// Gets or sets the <see cref="Exception"/>.
		/// </summary>
		public Exception Exception { get; internal set; }
	}
}
