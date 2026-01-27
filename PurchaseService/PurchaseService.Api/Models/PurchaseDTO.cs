
namespace PurchaseService.Api.Models
{
    public record PurchaseDto(int Id, int ClientId, decimal TotalAmount, DateTime CreatedAt, List<PurchaseItemDto> Items);
    public record PurchaseItemDto(int Id, int ProductId, int Quantity, decimal Price);
    public record CreatePurchaseDto(int ClientId, List<CreatePurchaseItemDto> Items);
    public record CreatePurchaseItemDto(int ProductId, int Quantity, decimal Price);
}
