using AspFirstMVC.Models;
using AspFirstMVC.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Minio;
using System;
using System.IO;
using System.Threading.Tasks;
using File = AspFirstMVC.Models.File;

namespace AspFirstMVC.Controllers
{
    public class FileController : Controller
    {
        private EFContext _context;
        private IWebHostEnvironment _env;
        private IConfiguration _configuration;


        public FileController(EFContext context, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.message = TempData["message"];
            return View();
        }

        //Grabar en BD
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

        //Grabar en disco
        public async Task<IActionResult> Upload2(UploadModel upload)
        {
            var filename = Path.Combine(_env.ContentRootPath, "uploads", upload.MyFile.FileName);
            await upload.MyFile.CopyToAsync(new FileStream(filename,FileMode.Create));

            TempData["message"] = "Archivo arriba";
            return RedirectToAction("Index");
        }

        //Grabar en S3 (Simple storage service) 
        public async Task<IActionResult> Upload3(UploadModel upload)
        {
            var filename = Path.Combine(_env.ContentRootPath, "uploads", upload.MyFile.FileName);
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                await upload.MyFile.CopyToAsync(fs);
            }

            try
            {
                var minioCliente = new MinioClient(_configuration["server"], _configuration["user"], _configuration["password"]).WithSSL();
                byte[] bs = await System.IO.File.ReadAllBytesAsync(filename);
                using (var ms = new MemoryStream(bs))
                {
                    await minioCliente.PutObjectAsync("video", upload.MyFile.FileName, ms, ms.Length, "application/octet-stream", null, null);
                }

                TempData["message"] = "Archivo arriba";
            }
            catch (Exception e)
            {
                TempData["message"] = $"Error: {e.Message}";
            }

            System.IO.File.Delete(filename);
            
            return RedirectToAction("Index");
        }
    }
}
