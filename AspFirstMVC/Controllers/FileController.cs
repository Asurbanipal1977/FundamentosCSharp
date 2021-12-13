using AspFirstMVC.Models;
using AspFirstMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using File = AspFirstMVC.Models.File;

namespace AspFirstMVC.Controllers
{
    public class FileController : Controller
    {
        private EFContext _context;
        public FileController(EFContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.message = TempData["message"];
            return View();
        }

        public async Task<IActionResult> Upload1 (UploadModel upload)
        {
            using (var db = _context)
            {
                using (var ms = new MemoryStream())
                {
                    var file = new File();
                    await upload.MyFile.CopyToAsync(ms);
                    file.FileDb = ms.ToArray();
                    db.Files.Add(file);
                    db.SaveChanges();
                }                
            }                          

            TempData["message"] = "Archivo arriba";
            return RedirectToAction("Index");
        }
    }
}
