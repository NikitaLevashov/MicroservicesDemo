using MediatR;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{
    public record CreateUpdatedCommand(
        string Name,
        decimal Price,
        int StockQuantity
    ) : IRequest<Product>;
}
