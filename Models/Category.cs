using System.ComponentModel.DataAnnotations;

namespace CrochetStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [StringLength(100, ErrorMessage = "اسم التصنيف يجب ألا يتجاوز 100 حرف")]
        [Display(Name = "اسم التصنيف")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "رابط الصورة")]
        public string? ImageUrl { get; set; }

        [Display(Name = "مفعل")]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
