using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace ProductService.Tests.Fixtures
{

    public class MsSqlFixture : IAsyncLifetime
    {
        public MsSqlContainer Container { get; }

        public MsSqlFixture()
        {
            Container = new MsSqlBuilder()
                .WithPassword("yourStrong(!)Password123")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await Container.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await Container.DisposeAsync();
        }
    }

}
