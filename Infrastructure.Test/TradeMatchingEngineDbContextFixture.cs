using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Infrastructure.Test
{
    public class TradeMatchingEngineDbContextFixture : IAsyncDisposable
    {
        public TradeMatchingEngineContext DbContext { get; }
        public ITestOutputHelper? Output { get; set; }

        public TradeMatchingEngineDbContextFixture()
        {
            var configuration = TestConfigHelper.GetIConfigurationRoot();
            var optionsBuilder = new DbContextOptionsBuilder<TradeMatchingEngineContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            optionsBuilder.LogTo(msg => Output?.WriteLine(msg));
            DbContext = new TradeMatchingEngineContext(optionsBuilder.Options);
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
        }

        public async ValueTask DisposeAsync()
        {
            await DbContext.DisposeAsync();
        }
    }
}