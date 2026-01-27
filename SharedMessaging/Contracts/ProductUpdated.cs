using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedMessaging.Contracts
{
    public record ProductUpdated(
        Guid EventId,
        DateTime OccurredAtUtc,
        string Version,
        int ProductId,
        string Name,
        decimal Price,
        int StockQuantity
    );
}
