using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;

namespace AspFirstMVC.Controllers
{
    public class PostController : Controller
    {
        private IConfiguration _configuration;
        public PostController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Create()
        {
            ViewBag.Message = TempData["Message"];
            var clave = _configuration["password"];
            return View();
        }

        [HttpPost]
        public IActionResult Create(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View("Create",post);
            }

            TempData["Message"] = "Guardado";
            return Redirect("Create");
        }
    }
}
