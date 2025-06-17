using System.ComponentModel.DataAnnotations;

namespace DTOs.CartDTOs
{
    public class AddCartItemRequest
    {
        [Required]
        public Guid ProductId { get; set; }
        public Guid? ProductVariantId { get; set; }
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
        public string? SelectedColor { get; set; }
        public string? SelectedSize { get; set; }
    }
}
