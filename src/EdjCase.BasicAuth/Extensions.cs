using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdjCase.BasicAuth
{
	public static class LogggerExtensions
	{
		public static void LogException(this ILogger logger, Exception ex, string message = null)
		{
			//Log error ignores the exception for some reason
			if (message != null)
			{
				message = $"{message}{Environment.NewLine}{ex}";
			}
			else
			{
				message = $"{ex}";
			}
			logger.LogError(new EventId(), ex, message);
		}
	}
}
