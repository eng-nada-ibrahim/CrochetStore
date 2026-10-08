using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetStore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [Required]
        [Display(Name = "الكمية")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "سعر القطعة")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "المقاس المختار")]
        [StringLength(50)]
        public string? SelectedSize { get; set; }

        [Display(Name = "ملاحظات وتفاصيل خاصة")]
        [StringLength(500)]
        public string? CustomNotes { get; set; }
    }
}
