using Microsoft.AspNetCore.Http;

namespace AspFirstMVC.Models.ViewModels
{
    public class UploadModel
    {
        public IFormFile MyFile { get; set; }
    }
}
