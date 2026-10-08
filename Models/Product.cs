using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المنتج مطلوب")]
        [StringLength(150, ErrorMessage = "اسم المنتج يجب ألا يتجاوز 150 حرف")]
        [Display(Name = "اسم المنتج")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "وصف المنتج مطلوب")]
        [Display(Name = "وصف المنتج")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "السعر مطلوب")]
        [Range(0.01, 100000.00, ErrorMessage = "السعر يجب أن يكون أكبر من الصفر")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "السعر (ج.م)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "الكمية المتاحة مطلوبة")]
        [Range(0, 10000, ErrorMessage = "الكمية يجب أن تكون 0 أو أكثر")]
        [Display(Name = "الكمية المتاحة")]
        public int StockQuantity { get; set; }

        [Display(Name = "صورة المنتج")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "نوع الخيط مطلوب")]
        [Display(Name = "نوع الخيط")]
        [StringLength(100)]
        public string YarnType { get; set; } = "قطن 100% طبيعي"; // e.g. قطن، أكريليك، صوف، مكرمية، كليم، قطيفة

        [Display(Name = "خيارات المقاسات المتاحة")]
        [StringLength(200)]
        public string SizeOptions { get; set; } = "S, M, L, XL, تفصيل خاص";

        [Required(ErrorMessage = "مدة التنفيذ مطلوبة")]
        [Range(1, 60, ErrorMessage = "مدة التجهيز بين 1 إلى 60 يوماً")]
        [Display(Name = "مدة التنفيذ والتجهيز اليدوي (أيام)")]
        public int ProcessingDays { get; set; } = 3;

        [Display(Name = "إمكانية التفصيل حسب الطلب")]
        public bool IsCustomizable { get; set; } = true;

        [Display(Name = "منتج مميز بالرئيسية")]
        public bool IsFeatured { get; set; } = false;

        [Required(ErrorMessage = "يرجى اختيار التصنيف")]
        [Display(Name = "التصنيف")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
