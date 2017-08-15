using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace EdjCase.BasicAuth
{
	public class BasicAuthHandleRequestContext : HandleRequestContext<BasicAuthOptions>
	{
		public BasicAuthHandleRequestContext(HttpContext context, AuthenticationScheme scheme, BasicAuthOptions options) 
			: base(context, scheme, options)
		{
		}
	}
}
