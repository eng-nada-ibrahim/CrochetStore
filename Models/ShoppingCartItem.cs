using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetStore.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        public string CartId { get; set; } = string.Empty;

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public int Quantity { get; set; } = 1;

        [Display(Name = "المقاس المختار")]
        [StringLength(50)]
        public string? SelectedSize { get; set; }

        [Display(Name = "ملاحظات مخصصة")]
        [StringLength(500)]
        public string? CustomNotes { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
