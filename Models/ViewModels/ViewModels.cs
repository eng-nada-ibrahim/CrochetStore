using System.ComponentModel.DataAnnotations;

namespace CrochetStore.Models.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? SelectedSize { get; set; }
        public string? CustomNotes { get; set; }
        public int ProcessingDays { get; set; }
        public string YarnType { get; set; } = string.Empty;

        public decimal TotalPrice => Price * Quantity;
    }

    public class ShoppingCartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal SubTotal => Items.Sum(i => i.TotalPrice);
        public decimal ShippingCost => Items.Any() ? 50.00m : 0.00m;
        public decimal GrandTotal => SubTotal + ShippingCost;
        public int MaxProcessingDays => Items.Any() ? Items.Max(i => i.ProcessingDays) : 0;
    }

    public class AddToCartRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; } = 1;

        public string? SelectedSize { get; set; }
        public string? CustomNotes { get; set; }
    }

    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "يرجى كتابة الاسم بالكامل")]
        [Display(Name = "الاسم الكامل")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "يرجى إدخال رقم الهاتف")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "يرجى إدخال عنوان الشحن بالتفصيل")]
        [Display(Name = "العنوان بالتفصيل")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "يرجى اختيار المدينة / المحافظة")]
        [Display(Name = "المدينة / المحافظة")]
        public string City { get; set; } = "القاهرة";

        [Required(ErrorMessage = "يرجى اختيار وسيلة الدفع")]
        [Display(Name = "طريقة الدفع")]
        public string PaymentMethod { get; set; } = "CashOnDelivery"; // CashOnDelivery or VodafoneCash

        [Display(Name = "ملاحظات إضافية للتوصيل")]
        public string? GeneralNotes { get; set; }

        public ShoppingCartViewModel? Cart { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalSales { get; set; }
        public int InProgressOrders { get; set; }
        public int PendingOrders { get; set; }
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public List<Order> RecentOrders { get; set; } = new List<Order>();
    }

    public class ProductFilterViewModel
    {
        public int? CategoryId { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public string? YarnType { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "بريد إلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "تذكرني")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "بريد إلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم هاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, ErrorMessage = "يجب أن تكون كلمة المرور 6 أحرف على الأقل", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
