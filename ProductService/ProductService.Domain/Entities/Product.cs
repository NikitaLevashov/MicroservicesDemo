
using ProductService.Domain.Abstractions;
using ProductService.Domain.Events;
using ProductService.Domain.ValueObjects;

public class Product
{
    private readonly List<IDomainEvent> _events = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

    public int Id { get; private set; }
    public string Name { get; private set; } = default!;
    public Price Price { get; private set; }
    public int StockQuantity { get; private set; }

    private Product() { }

    public static Product Create(string name, Price price, int stock)
    {
        var p = new Product();
        p.SetName(name);
        p.SetPrice(price);
        p.SetStock(stock);

        //p.AddDomainEvent(new ProductCreatedDomainEvent(p.Id, name, price.Value, stock));
        return p;
    }

    public void Rename(string name)
    {
        if (Name == name) return;
        var old = Name;
        SetName(name);
        //AddDomainEvent(new ProductUpdatedDomainEvent(Id, Name, Price.Value, StockQuantity));
    }

    public void ChangePrice(Price price)
    {
        if (Price == price) return;
        var old = Price;
        SetPrice(price);
        //AddDomainEvent(new ProductUpdatedDomainEvent(Id, Name, Price.Value, StockQuantity));
    }

    public void AdjustStock(int stock)
    {
        if (StockQuantity == stock) return;
        var old = StockQuantity;
        SetStock(stock);
        //AddDomainEvent(new ProductUpdatedDomainEvent(Id, Name, Price.Value, StockQuantity));
    }

    // --- инварианты (минимум кода) ---
    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));
        Name = name.Trim();
    }

    private void SetPrice(Price price)
    {
        // Price уже валидирует себя; добавьте свои правила при необходимости
        Price = price;
    }

    private void SetStock(int stock)
    {
        if (stock < 0) throw new ArgumentOutOfRangeException(nameof(stock));
        StockQuantity = stock;
    }

    // --- события ---
    public void AddDomainEvent(IDomainEvent e) => _events.Add(e);
    public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents()
    {
        var copy = _events.ToArray();
        _events.Clear();
        return copy;
    }
}
