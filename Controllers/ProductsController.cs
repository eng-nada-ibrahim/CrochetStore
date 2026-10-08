using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetStore.Data;
using CrochetStore.Models.ViewModels;

namespace CrochetStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Products
        public async Task<IActionResult> Index(int? categoryId, string? searchTerm, string? sortBy, string? yarnType)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm) || p.YarnType.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(yarnType))
            {
                query = query.Where(p => p.YarnType.Contains(yarnType));
            }

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "name" => query.OrderBy(p => p.Name),
                "processing_asc" => query.OrderBy(p => p.ProcessingDays),
                _ => query.OrderByDescending(p => p.Id)
            };

            var categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
            var products = await query.ToListAsync();

            var viewModel = new ProductFilterViewModel
            {
                CategoryId = categoryId,
                SearchTerm = searchTerm,
                SortBy = sortBy,
                YarnType = yarnType,
                Products = products,
                Categories = categories
            };

            return View(viewModel);
        }

        // GET: /Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Related Products from same category
            var relatedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }
    }
}
