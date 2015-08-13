using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace edjCase.BasicAuth.Sample.Controllers
{
	[Authorize]
	[Route("Home")]
	public class HomeController : Controller
	{
		[HttpGet("Index")]
		public IActionResult Index()
		{
			return new EmptyResult();
		}
	}
}
