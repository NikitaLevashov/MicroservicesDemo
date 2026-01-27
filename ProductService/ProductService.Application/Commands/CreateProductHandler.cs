using MediatR;
using ProductService.Application.Abstractions;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Domain.Events;
using ProductService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{

    public class CreateProductHandler : IRequestHandler<CreateUpdatedCommand, Product>
    {
        private readonly IProductRepository _repo;
        private readonly IDomainEventDispatcher _dispatcher;

        public CreateProductHandler(IProductRepository repo, IDomainEventDispatcher dispatcher)
        {
            _repo = repo;
            _dispatcher = dispatcher;
        }

        public async Task<Product> Handle(CreateUpdatedCommand cmd, CancellationToken ct)
        {
            var product = Product.Create(
              cmd.Name,
              new Price(cmd.Price),
              cmd.StockQuantity);

            await _repo.CreateAsync(product);

            //await newProduct;

            //var productResult = newProduct.Result;

            //await _dispatcher.DispatchAsync(productResult.DomainEvents, ct);
            //productResult.DequeueDomainEvents();


            product.AddDomainEvent(new ProductCreatedDomainEvent(
                Id: product.Id,
                Name: product.Name,
                Price: product.Price.Value,
                StockQuantity: product.StockQuantity));

            var events = product.DequeueDomainEvents();
            await _dispatcher.DispatchAsync(events, ct);


            return product;
        }
    }

}
