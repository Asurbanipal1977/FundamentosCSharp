using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Models;

namespace AspFirstMVC.Controllers
{
    public class PostController : Controller
    {
        private IConfiguration _configuration;
        private IHubContext<PostHub> _hubContext;
        public PostController(IConfiguration configuration, IHubContext<PostHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;
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

            _hubContext.Clients.All.SendAsync("Receive", post.Id, post.Title);

            TempData["Message"] = "Guardado";
            return Redirect("Create");
        }
    }
}
