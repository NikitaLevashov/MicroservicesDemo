namespace ProductService.Api.Models
{
    public record ProductDto(int Id, string Name, decimal Price, int StockQuantity);
    public record CreateProductDto(string Name, decimal Price, int StockQuantity);
    public record UpdateProductDto(string Name, decimal Price, int StockQuantity);
}
