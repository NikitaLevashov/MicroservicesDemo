//using MassTransit;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using NotificationService.Application.Notifications.Commands.CreateNotification;
//using SharedMessaging.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NotificationService.Infrastructure.Messagimg
//{

//    public sealed class ProductCreatedConsumer : IConsumer<ProductCreated>
//    {
//        private readonly ILogger<ProductCreatedConsumer> _logger;
//        private readonly IMediator _mediator;

//        public ProductCreatedConsumer(ILogger<ProductCreatedConsumer> logger, IMediator mediator)
//        {
//            _logger = logger;
//            _mediator = mediator;
//        }

//        public async Task Consume(ConsumeContext<ProductCreated> context)
//        {
//            var evt = context.Message;
//            _logger.LogInformation("Consuming ProductCreated: {ProductId}", evt.ProductId);

//            var type = "ProductCreated";
//            var recipient = "team@example.com"; // вынесешь в конфиг при необходимости
//            var message = $"Product created #{evt.ProductId}: {evt.Name} ({evt.Price}, qty={evt.StockQuantity})";

//            await _mediator.Send(new CreateNotificationCommand(type, recipient, message), context.CancellationToken);
//        }
//    }

//}
