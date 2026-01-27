using MediatR;
using ProductService.Application.Abstractions;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IProductRepository _repo;
        private readonly IDomainEventDispatcher _dispatcher;

        public UpdateProductHandler(IProductRepository repo, IDomainEventDispatcher dispatcher)
        {
            _repo = repo;
            _dispatcher = dispatcher;
        }
        public async Task<Product> Handle(UpdateProductCommand cmd, CancellationToken ct)
        {
            var product = await _repo.GetByIdAsync(cmd.id);

            if (product is null) return null;
            
            //var updatedProduct = Product.Create(
            //   cmd.Name,
            //   new Price(cmd.Price),
            //   cmd.StockQuantity
           //);

            product.Rename(cmd.Name);
            product.ChangePrice(new Price(cmd.Price));
            product.AdjustStock(cmd.StockQuantity);

           await _repo.UpdateAsync(product);            

           await _dispatcher.DispatchAsync(product.DomainEvents, ct);
           product.DequeueDomainEvents();

           return product;
        }
    }
}
