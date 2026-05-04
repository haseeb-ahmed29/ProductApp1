using System.ComponentModel.DataAnnotations;

namespace ProductApp.Models
{
    public class Product
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100)]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01 and 99999.99")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Product Image")]
        public string? ImagePath { get; set; }
    }
}