using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data.Models;
using TeknoForce.Data;
using System.IO;

namespace TeknoForce.Pages.Admin.Products
{
    public class EditModel : AdminBasePageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = new();

        [BindProperty]
        public ProductSpecification Specification { get; set; } = new();

        [BindProperty]
        public List<IFormFile> MediaFiles { get; set; } = new();

        public List<ProductMedia> ProductMedias { get; set; } = new();

        public SelectList BrandList { get; set; } = null!;
        public SelectList CategoryList { get; set; } = null!;

        public IActionResult OnGet(int? id)
        {
            LoadDropdowns();

            if (id == null)
            {
                Product = new Product { IsActive = true };
                Specification = new ProductSpecification();
                return Page();
            }

            Product = _context.Products
                .Include(p => p.Specification)
                .FirstOrDefault(p => p.ProductId == id);

            if (Product == null)
                return NotFound();

            Specification = Product.Specification ?? new ProductSpecification();

            ProductMedias = _context.ProductMedias
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

            if (Product.ProductId == 0)
            {
                Product.CreatedDate = DateTime.Now;
                _context.Products.Add(Product);
                await _context.SaveChangesAsync();

                Specification.ProductId = Product.ProductId;
                _context.ProductSpecifications.Add(Specification);
                await _context.SaveChangesAsync();
            }
            else
            {
                var productFromDb = _context.Products
                    .Include(p => p.Specification)
                    .FirstOrDefault(p => p.ProductId == Product.ProductId);

                if (productFromDb == null)
                    return NotFound();

                productFromDb.Name = Product.Name;
                productFromDb.BrandId = Product.BrandId;
                productFromDb.CategoryId = Product.CategoryId;
                productFromDb.Price = Product.Price;
                productFromDb.Stock = Product.Stock;
                productFromDb.IsActive = Product.IsActive;

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

                await _context.SaveChangesAsync();
            }

            // MEDYA YÜKLEME
            if (MediaFiles != null && MediaFiles.Any())
            {
                var uploadPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/products"
                );

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);


                bool hasCover = _context.ProductMedias
                    .Any(x => x.ProductId == Product.ProductId && x.IsCover);

                foreach (var file in MediaFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadPath, fileName);

                        using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);

                        var media = new ProductMedia
                        {
                            ProductId = Product.ProductId,
                            FilePath = "/uploads/products/" + fileName,
                            MediaType = file.ContentType.StartsWith("video") ? "Video" : "Image",
                            IsCover = !hasCover,
                            CreatedDate = DateTime.Now
                        };

                        hasCover = true;
                        _context.ProductMedias.Add(media);
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostSetCover(int mediaId, int productId)
        {
            var medias = _context.ProductMedias
                .Where(x => x.ProductId == productId)
                .ToList();

            foreach (var media in medias)
            {
                media.IsCover = media.ProductMediaId == mediaId;
            }

            _context.SaveChanges();

            return RedirectToPage(new { id = productId });
        }

        public IActionResult OnPostDeleteMedia(int mediaId, int productId)
        {
            var media = _context.ProductMedias
                .FirstOrDefault(x => x.ProductMediaId == mediaId);

            if (media == null)
                return RedirectToPage(new { id = productId });

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                media.FilePath.TrimStart('/')
            );

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            bool wasCover = media.IsCover;

            _context.ProductMedias.Remove(media);
            _context.SaveChanges();

            if (wasCover)
            {
                var newCover = _context.ProductMedias
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
