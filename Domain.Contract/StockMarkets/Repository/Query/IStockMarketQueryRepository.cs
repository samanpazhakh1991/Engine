using Framework.Contracts.GenericRepositories;

namespace Domain.Contract.StockMarkets.Repository.Query
{
    public interface IStockMarketQueryRepository :
        IQueryRepository<StockMarketMatchEngine, IStockMarketMatchEngine, Guid>
    {
    }
}