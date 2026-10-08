using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetStore.Data;
using CrochetStore.Models;
using CrochetStore.Models.ViewModels;
using CrochetStore.Services;

namespace CrochetStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context;

        public CartController(ICartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        // POST: /Cart/AddToCart (AJAX support)
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            if (request == null || request.ProductId <= 0)
            {
                return Json(new { success = false, message = "طلب غير صالح" });
            }

            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return Json(new { success = false, message = "المنتج غير موجود" });
            }

            if (product.StockQuantity < request.Quantity)
            {
                return Json(new { success = false, message = "الكمية المطلوبة غير متوفرة حالياً في المخزن" });
            }

            _cartService.AddToCart(
                request,
                product.Name,
                product.Price,
                product.ImageUrl,
                product.ProcessingDays,
                product.YarnType
            );

            int totalCount = _cartService.GetTotalCount();
            return Json(new { success = true, message = "تمت إضافة القطعة إلى سلة التسوق بنجاح!", cartCount = totalCount });
        }

        // POST: /Cart/UpdateQuantity (AJAX support)
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, string? selectedSize, int quantity)
        {
            _cartService.UpdateQuantity(productId, selectedSize, quantity);
            var cart = _cartService.GetCart();
            return Json(new { 
                success = true, 
                cartCount = _cartService.GetTotalCount(),
                subTotal = cart.SubTotal,
                shippingCost = cart.ShippingCost,
                grandTotal = cart.GrandTotal
            });
        }

        // POST: /Cart/RemoveItem
        [HttpPost]
        public IActionResult RemoveItem(int productId, string? selectedSize)
        {
            _cartService.RemoveFromCart(productId, selectedSize);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Cart/GetCount (AJAX)
        [HttpGet]
        public IActionResult GetCount()
        {
            return Json(new { count = _cartService.GetTotalCount() });
        }

        // GET: /Cart/Checkout
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCart();
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index", "Products");
            }

            var model = new CheckoutViewModel
            {
                Cart = cart
            };

            return View(model);
        }

        // POST: /Cart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var cart = _cartService.GetCart();
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index", "Products");
            }

            model.Cart = cart;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var order = new Order
            {
                CustomerName = model.CustomerName,
                Phone = model.Phone,
                ShippingAddress = model.ShippingAddress,
                City = model.City,
                PaymentMethod = model.PaymentMethod == "VodafoneCash" ? "فودافون كاش / إنستاباي" : "الدفع عند الاستلام",
                GeneralNotes = model.GeneralNotes,
                OrderDate = DateTime.UtcNow,
                Status = "قيد الانتظار",
                TotalAmount = cart.GrandTotal
            };

            foreach (var item in cart.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    SelectedSize = item.SelectedSize,
                    CustomNotes = item.CustomNotes
                });

                // Update product stock if available
                var prod = await _context.Products.FindAsync(item.ProductId);
                if (prod != null)
                {
                    prod.StockQuantity = Math.Max(0, prod.StockQuantity - item.Quantity);
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear Cart Session
            _cartService.ClearCart();

            return RedirectToAction("OrderConfirmation", new { id = order.Id });
        }

        // GET: /Cart/OrderConfirmation/5
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
