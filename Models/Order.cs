using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required(ErrorMessage = "اسم العميل مطلوب")]
        [Display(Name = "اسم العميل")]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "عنوان الشحن مطلوب")]
        [Display(Name = "عنوان الشحن بالتفصيل")]
        [StringLength(250)]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "المدينة / المحافظة مطلوبة")]
        [Display(Name = "المدينة / المحافظة")]
        [StringLength(50)]
        public string City { get; set; } = string.Empty;

        [Display(Name = "تاريخ الطلب")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "إجمالي المبلغ")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "حالة الطلب")]
        [StringLength(50)]
        public string Status { get; set; } = "قيد الانتظار"; 
        // حالات الطلب: قيد الانتظار، جاري التنفيذ اليدوي، تم الشحن، تم التسليم، ملغي

        [Required(ErrorMessage = "يرجى اختيار طريقة الدفع")]
        [Display(Name = "طريقة الدفع")]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = "الدفع عند الاستلام"; 
        // CashOnDelivery (الدفع عند الاستلام)، VodafoneCash (فودافون كاش / إنستاباي)

        [Display(Name = "ملاحظات إضافية على الطلب")]
        [StringLength(500)]
        public string? GeneralNotes { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
