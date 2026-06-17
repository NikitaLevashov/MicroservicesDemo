using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProductService.Api.Controllers.ProductService.Api.Controllers;
using ProductService.Domain.ValueObjects;
using ProductService.Infrastructure.Persistence;
using ProductService.Tests.Fixtures;
using System.Security.Claims;
using ProductService.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;
using ProductService.Infrastructure.Persistence;
using System.Security.Claims;
using Xunit;
using ProductService.Api.Controllers;
using ProductService.Domain.Entities;

public class GetAllProductsTests : IClassFixture<MsSqlFixture>
{
    private readonly MsSqlFixture _fixture;

    public GetAllProductsTests(MsSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAll_Should_Return_Products_From_Db()
    {
        // 🔹 1. DbContext
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseSqlServer(_fixture.Container.GetConnectionString())
            .Options;

        var connectionString = _fixture.Container.GetConnectionString();

        // 🔹 2. Создаём БД и seed (ВАЖНО: через Create)
        await using (var context = new ProductDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();

            var p1 = Product.Create("Product1", new Price(10), 1);
            var p2 = Product.Create("Product2", new Price(20), 2);

            context.Products.AddRange(p1, p2);
            await context.SaveChangesAsync();
        }

        // 🔹 3. Новый контекст (чтобы не было EF Cache)
        await using var context2 = new ProductDbContext(options);
        var repo = new ProductRepository(context2);

        // 🔹 4. Cache
        var cache = new MemoryCache(new MemoryCacheOptions());

        // 🔹 5. Controller
        var controller = new ProductsController(
            mediator: null!,
            service: repo,
            claimsPrincipal: new ClaimsPrincipal(),
            memoryCache: cache
        );

        // 🔹 6. Act
        var result = await controller.GetAll();

        // 🔹 7. Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var products = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);

        Assert.Equal(2, products.Count());
    }
}

