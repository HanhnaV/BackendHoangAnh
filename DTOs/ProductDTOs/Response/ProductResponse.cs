namespace DTOs.ProductDTOs.Response
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
    }
}
