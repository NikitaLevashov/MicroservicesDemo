using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Abstractions
{
    public interface IDomainEvent
    {
        // Часто оставляют пустым (маркерный интерфейс).
        // По желанию можно добавить общие поля:
        // DateTime OccurredAtUtc { get; }
    }

}
