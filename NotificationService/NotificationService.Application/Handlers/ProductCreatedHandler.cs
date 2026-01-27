
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Domain;
using NotificationService.Domain.Notifications;
using SharedMessaging.Contracts;

//public class ProductCreatedHandler
//{
//    private readonly INotificationService _service;
//    private readonly ILogger<ProductCreatedHandler> _logger;

//    public ProductCreatedHandler(INotificationService service, ILogger<ProductCreatedHandler> logger)
//    {
//        _service = service;
//        _logger = logger;
//    }

//    public async Task Handle(ProductCreated evt, CancellationToken ct = default)
//    {
//        _logger.LogInformation("Handling ProductCreated in Application: {ProductId}", evt.ProductId);
//        Console.WriteLine("Test ProductCreatedHandler");

//        var notification = new Notification
//        {
//            Type = "ProductCreated",
//            Recipient = "team@example.com",
//            Message = $"Product created #{evt.ProductId}: {evt.Name} ({evt.Price}, qty={evt.StockQuantity})",
//            Status = "Pending"
//        };

//        await _service.CreateAsync(notification);
//    }
//}
