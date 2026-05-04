using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApp.Data;
using ProductApp.Models;


namespace StudentManagementSystem.Controllers
{

    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Read - list all the products
        public async Task<IActionResult> Index()
        {
            var product = await _context.Products.ToListAsync();
            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        //Create: Save Product + Image
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,Price")] Product product,
            IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                //Handle image upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(
                        _env.WebRootPath, "images", "products");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueName = Guid.NewGuid().ToString()
                        + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    product.ImagePath = uniqueName;
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View();
        }

        // Delete Confirm page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View();
        }

        //Delete: Confirmed Page
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                //Delete the image file
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    var imgPath = Path.Combine(
                        _env.WebRootPath, "images", "Products",
                        product.ImagePath);
                    if (System.IO.File.Exists(imgPath))
                        System.IO.File.Delete(imgPath);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}