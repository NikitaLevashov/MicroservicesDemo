
// Shared/SharedMessaging.Contracts/ProductCreated.cs
namespace SharedMessaging.Contracts;

/// <summary>
/// Событие "товар создан".
/// Это интеграционное событие (межсервисное), поэтому не тянем сюда доменную сущность Product.
/// Передаём только необходимые для других сервисов поля.
/// </summary>
public record ProductCreated(
    Guid EventId,
    DateTime OccurredAtUtc,
    string Version,
    int ProductId,
    string Name,
    decimal Price,
    int StockQuantity
);
