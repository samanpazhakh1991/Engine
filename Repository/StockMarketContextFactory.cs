using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure
{
    public class StockMarketContextFactory : IDesignTimeDbContextFactory<TradeMatchingEngineContext>
    {
        public TradeMatchingEngineContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TradeMatchingEngineContext>();
            optionsBuilder.UseSqlServer(args[0]);

            return new TradeMatchingEngineContext(optionsBuilder.Options);
        }
    }
}
