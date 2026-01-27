using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class UserTest
    {
        public class UserCompanyDto
        {
            public int? UserId { get; set; } // nullable
            public string? Name { get; set; }
            public int CompanyId { get; set; }
            public string? Company { get; set; }
        }

        // Test
        [Table("Country")]
        public class Country
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Region { get; set; }
            public string? ISO { get; set; } = null!;
            public List<Company> Companies { get; set; } = new();
        }

        public class Company
        {
            public int Id { get; set; }
            public string? Name { get; set; } // название компании

            public Country? Country { get; set; } = new();

            public int CountryId { get; set; } = new();

            public List<User> Users { get; set; } = new();
        }

        public class User
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int Age { get; set; }

            public int? CompanyInfoKey { get; set; }      // внешний ключ
            public Company? Company { get; set; }    // навигационное свойство
        }
    }
}
