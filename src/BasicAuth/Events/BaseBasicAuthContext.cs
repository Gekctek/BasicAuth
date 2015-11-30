using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;

namespace edjCase.BasicAuth.Events
{
	public class BaseBasicAuthContext : BaseControlContext
	{
		public BasicAuthOptions Options { get; }

		public BaseBasicAuthContext(HttpContext context, BasicAuthOptions options)
			: base(context)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			this.Options = options;
		}

	}
}