using ProductService.Domain.Abstractions;

namespace ProductService.Domain.Events
{
    public record ProductCreatedDomainEvent(
        int Id,
        string Name,
        decimal Price,
        int StockQuantity
    ) : IDomainEvent;

}
