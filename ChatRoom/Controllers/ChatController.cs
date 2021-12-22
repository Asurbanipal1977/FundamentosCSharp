using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ChatRoom.Controllers
{
    public class ChatController : Controller
    {
        public static Dictionary<int, string> rooms = new Dictionary<int, string>()
        {
            {1,"Deporte"},
            {2,"Música"},
            {3,"Actualidad"}
        };
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Room(int room)
        {
            return View("Room", room);
        }
    }
}
