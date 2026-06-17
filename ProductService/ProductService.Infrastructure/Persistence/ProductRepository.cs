using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Persistence;
using static ProductService.Domain.Entities.UserTest;

namespace ProductService.Infrastructure.Persistence
{   

    public class ProductRepository : IProductRepository, IDisposable
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync() => await _context.Products.ToListAsync();

        public async Task<List<User>> TestApiAsync()
        {
            var result = _context.Users.AsNoTracking<User>().
                Include(c => c.Company).
                Include(c => c.Company).
                ThenInclude(c => c!.Country).
                ToList();

            //var result = _context.Users.Include(c => c.Company).Where(u => u.Company!.Id == 1);

            //var result1 = (from user in _context.Users.Include(p => p.Company)
            //               where user.Company!.Id == 1
            //               select user).ToListAsync();

            //var users = _context.Users.Join(_context.Companies, // второй набор
            //        u => u.CompanyInfoKey, // свойство-селектор объекта из первого набора
            //        c => c.Id, // свойство-селектор объекта из второго набора
            //        (u, c) => new // результат
            //        {
            //            Name = u.Name,
            //            Company = c.Name,
            //        });

            var context = _context.ChangeTracker.Entries();
            var countTracked = _context.ChangeTracker.Entries().Count();

            var resultSQL = _context.UserCompanyDto
                .FromSqlInterpolated($"SELECT u.Id as UserId, u.Name, c.Id AS CompanyId, c.Name AS Company FROM [ProductDb].[userstore].[Users] u RIGHT JOIN Companies c ON u.CompanyInfoKey = c.Id");

            SqlParameter paramF = new SqlParameter("@age", 30);
            var usersF = await _context.Users.FromSqlRaw("SELECT * FROM GetUsersByAge (@age)", paramF).ToListAsync();
            //foreach (var item in resultSQL)
            //{
            //    yield return item;
            //}

            SqlParameter paramSP = new("@name", "Microsoft");
            var usersSP = await _context.Users.FromSqlRaw("GetUsersByCompany @name", paramSP).ToListAsync();

            SqlParameter param = new()
            {
                ParameterName = "@userName",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Output,
                Size = 50
            };
            _context.Users.FromSqlRaw("GetUserWithMaxAge @userName OUT", param);

            return usersSP;
        }

        public async Task<Product?> GetByIdAsync(int id) => await _context.Products.FindAsync(id);

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
