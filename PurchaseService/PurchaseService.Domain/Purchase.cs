using PurchaseService.Domain;

public class Purchase
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
}