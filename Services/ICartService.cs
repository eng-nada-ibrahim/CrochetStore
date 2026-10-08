using CrochetStore.Models.ViewModels;

namespace CrochetStore.Services
{
    public interface ICartService
    {
        ShoppingCartViewModel GetCart();
        void AddToCart(AddToCartRequest request, string productName, decimal price, string? imageUrl, int processingDays, string yarnType);
        void UpdateQuantity(int productId, string? selectedSize, int quantity);
        void RemoveFromCart(int productId, string? selectedSize);
        void ClearCart();
        int GetTotalCount();
    }
}
