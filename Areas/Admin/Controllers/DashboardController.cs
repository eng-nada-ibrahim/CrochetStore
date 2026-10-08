using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetStore.Data;
using CrochetStore.Models.ViewModels;

namespace CrochetStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalOrders = await _context.Orders.CountAsync();
            var totalSales = await _context.Orders.Where(o => o.Status != "ملغي").SumAsync(o => (decimal?)o.TotalAmount) ?? 0;
            var inProgressOrders = await _context.Orders.CountAsync(o => o.Status == "جاري التنفيذ اليدوي");
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == "قيد الانتظار");
            var totalProducts = await _context.Products.CountAsync();
            var lowStockProducts = await _context.Products.CountAsync(p => p.StockQuantity <= 3);

            var recentOrders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();

            var viewModel = new AdminDashboardViewModel
            {
                TotalOrders = totalOrders,
                TotalSales = totalSales,
                InProgressOrders = inProgressOrders,
                PendingOrders = pendingOrders,
                TotalProducts = totalProducts,
                LowStockProducts = lowStockProducts,
                RecentOrders = recentOrders
            };

            return View(viewModel);
        }
    }
}
