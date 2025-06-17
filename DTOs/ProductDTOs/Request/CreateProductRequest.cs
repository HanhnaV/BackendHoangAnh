using System.ComponentModel.DataAnnotations;

namespace DTOs.ProductDTOs.Request
{
    public class CreateProductRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal? SalePrice { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
