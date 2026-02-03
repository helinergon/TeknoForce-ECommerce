//products klasöründeki edit.cshtml.cs kodları:
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data.Models;
using TeknoForce.Data;
using System.IO;


namespace TeknoForce.Pages.Products
{
    public class EditModel : PageModel
    {
        public List<ProductImage> ProductImages { get; set; } = new();

        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        // ŞİMDİLİK SADECE MODEL TUTUYORUZ
        [BindProperty]
        public Product Product { get; set; } = new();
        [BindProperty]
        public List<IFormFile> UploadedImages { get; set; } = new();

        [BindProperty]
        public ProductSpecification Specification { get; set; } = new();
        public SelectList BrandList { get; set; } = null!;
        public SelectList CategoryList { get; set; } = null!;

        public IActionResult OnGet(int? id)
        {
            LoadDropdowns();

            // CREATE
            if (id == null)
            {
                Product = new Product
                {
                    IsActive = true
                };

                Specification = new ProductSpecification();
                return Page();
            }

            // EDIT → DB'DEN OKU
            Product = _context.Products
                .Include(p => p.Specification)
                .FirstOrDefault(p => p.ProductId == id);

            if (Product == null)
                return NotFound();

            Specification = Product.Specification ?? new ProductSpecification();

            // 🔥 GÖRSELLERİ BURADA ÇEK
            ProductImages = _context.ProductImages
                .Where(x => x.ProductId == Product.ProductId)
                .OrderByDescending(x => x.IsCover)
                .ThenBy(x => x.CreatedDate)
                .ToList();

            return Page();
        }

        private void LoadDropdowns()
        {
            BrandList = new SelectList(
                _context.Brands.Where(b => b.IsActive),
                "BrandId",
                "Name"
            );

            CategoryList = new SelectList(
                _context.Categories.Where(c => c.IsActive),
                "CategoryId",
                "Name"
            );
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return Page();
            }

            // CREATE
            if (Product.ProductId == 0)
            {
                Product.CreatedDate = DateTime.Now;

                _context.Products.Add(Product);
                _context.SaveChanges(); // ID burada oluşur

                Specification.ProductId = Product.ProductId;
                _context.ProductSpecifications.Add(Specification);

                _context.SaveChanges();
            }
            // UPDATE
            else
            {
                var productFromDb = _context.Products
                    .Include(p => p.Specification)
                    .FirstOrDefault(p => p.ProductId == Product.ProductId);

                if (productFromDb == null)
                    return NotFound();

                // Product alanları
                productFromDb.Name = Product.Name;
                productFromDb.BrandId = Product.BrandId;
                productFromDb.CategoryId = Product.CategoryId;
                productFromDb.Price = Product.Price;
                productFromDb.Stock = Product.Stock;
                productFromDb.IsActive = Product.IsActive;

                // Specification
                if (productFromDb.Specification == null)
                {
                    Specification.ProductId = productFromDb.ProductId;
                    _context.ProductSpecifications.Add(Specification);
                }
                else
                {
                    productFromDb.Specification.MaxNozzle = Specification.MaxNozzle;
                    productFromDb.Specification.MotorPowerHP = Specification.MotorPowerHP;
                    productFromDb.Specification.WeightKg = Specification.WeightKg;
                    productFromDb.Specification.SetContent = Specification.SetContent;
                    productFromDb.Specification.UsageAreas = Specification.UsageAreas;
                    productFromDb.Specification.UsableMaterials = Specification.UsableMaterials;
                }

                _context.SaveChanges();

                if (UploadedImages != null && UploadedImages.Any())
                {
                    var uploadPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/uploads/products"
                    );

                    bool hasCover = _context.ProductImages
                        .Any(x => x.ProductId == Product.ProductId && x.IsCover);

                    foreach (var file in UploadedImages)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploadPath, fileName);

                            using var stream = new FileStream(filePath, FileMode.Create);
                            await file.CopyToAsync(stream);

                            var image = new ProductImage
                            {
                                ProductId = Product.ProductId,
                                ImagePath = "/uploads/products/" + fileName,
                                IsCover = !hasCover, // ilk görsel kapak olur
                                CreatedDate = DateTime.Now
                            };

                            hasCover = true;

                            _context.ProductImages.Add(image);
                        }
                    }

                    await _context.SaveChangesAsync();
                }

            }

            return RedirectToPage("./Index");
        }
        public IActionResult OnPostSetCover(int imageId, int productId)
        {
            var images = _context.ProductImages
                .Where(x => x.ProductId == productId)
                .ToList();

            foreach (var img in images)
            {
                img.IsCover = img.ProductImageId == imageId;
            }

            _context.SaveChanges();

            return RedirectToPage(new { id = productId });
        }
        public IActionResult OnPostDeleteImage(int imageId, int productId)
        {
            var image = _context.ProductImages
                .FirstOrDefault(x => x.ProductImageId == imageId);

            if (image == null)
                return RedirectToPage(new { id = productId });

            // 📁 Fiziksel dosyayı sil
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                image.ImagePath.TrimStart('/')
            );

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            bool wasCover = image.IsCover;

            _context.ProductImages.Remove(image);
            _context.SaveChanges();

            // 🔄 Eğer kapak silindiyse → yenisini ata
            if (wasCover)
            {
                var newCover = _context.ProductImages
                    .Where(x => x.ProductId == productId)
                    .OrderBy(x => x.CreatedDate)
                    .FirstOrDefault();

                if (newCover != null)
                {
                    newCover.IsCover = true;
                    _context.SaveChanges();
                }
            }

            return RedirectToPage(new { id = productId });
        }



    }
}
