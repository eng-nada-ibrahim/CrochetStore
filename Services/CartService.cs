using System.Text.Json;
using CrochetStore.Models.ViewModels;

namespace CrochetStore.Services
{
    public class CartService : ICartService
    {
        private const string CartSessionKey = "CrochetShoppingCart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext?.Session 
            ?? throw new InvalidOperationException("Session is not available.");

        public ShoppingCartViewModel GetCart()
        {
            var cartJson = Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new ShoppingCartViewModel();
            }

            try
            {
                return JsonSerializer.Deserialize<ShoppingCartViewModel>(cartJson) ?? new ShoppingCartViewModel();
            }
            catch
            {
                return new ShoppingCartViewModel();
            }
        }

        private void SaveCart(ShoppingCartViewModel cart)
        {
            var json = JsonSerializer.Serialize(cart);
            Session.SetString(CartSessionKey, json);
        }

        public void AddToCart(AddToCartRequest request, string productName, decimal price, string? imageUrl, int processingDays, string yarnType)
        {
            var cart = GetCart();
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId && i.SelectedSize == request.SelectedSize);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                if (!string.IsNullOrWhiteSpace(request.CustomNotes))
                {
                    existingItem.CustomNotes = request.CustomNotes;
                }
            }
            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = request.ProductId,
                    ProductName = productName,
                    ProductImage = imageUrl,
                    Price = price,
                    Quantity = request.Quantity,
                    SelectedSize = request.SelectedSize,
                    CustomNotes = request.CustomNotes,
                    ProcessingDays = processingDays,
                    YarnType = yarnType
                });
            }

            SaveCart(cart);
        }

        public void UpdateQuantity(int productId, string? selectedSize, int quantity)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId && i.SelectedSize == selectedSize);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Items.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                SaveCart(cart);
            }
        }

        public void RemoveFromCart(int productId, string? selectedSize)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId && i.SelectedSize == selectedSize);
            if (item != null)
            {
                cart.Items.Remove(item);
                SaveCart(cart);
            }
        }

        public void ClearCart()
        {
            Session.Remove(CartSessionKey);
        }

        public int GetTotalCount()
        {
            var cart = GetCart();
            return cart.Items.Sum(i => i.Quantity);
        }
    }
}
