using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using TravelWebsite.Data;

namespace TravelWebsite.Controllers
{
    public class ImageTestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageTestController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            // Lấy tất cả destination hiện có trong DB
            var destinations = _context.Destinations
                .Where(d => d.IsActive)
                .OrderBy(d => d.Id)
                .ToList();

            // Lấy danh sách tệp tin hình ảnh có trong thư mục destinations
            string destinationsPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "destinations");
            var imageFiles = new List<string>();
            
            if (Directory.Exists(destinationsPath))
            {
                imageFiles = Directory.GetFiles(destinationsPath)
                    .Select(Path.GetFileName)
                    .ToList();
            }

            ViewData["Destinations"] = destinations;
            ViewData["ImageFiles"] = imageFiles;

            return View();
        }
    }
}
