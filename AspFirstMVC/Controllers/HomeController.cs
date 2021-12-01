using AspFirstMVC.Models;
using AspFirstMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspFirstMVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IServicio _servicio;
		

		public HomeController(ILogger<HomeController> logger, IServicio servicio)
		{
			_logger = logger;
			_servicio = servicio;
		}

		public async Task<IActionResult> Index()
		{
			List<Post> lista = new List<Post>();
			try
			{
				lista = await _servicio.ListarPost();
			}
			catch (Exception e)
			{
				ModelState.AddModelError("ErrorMessage", $"Es un error: {e.Message}");
			}

			return View("Index",lista);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
