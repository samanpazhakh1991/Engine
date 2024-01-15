using Framework.Contracts.GenericRepositories;

namespace Domain.Contract.StockMarkets.Repository.Command
{
    public interface IStockMarketCommandRepository :
        ICommandRepository<StockMarketMatchEngine, IStockMarketMatchEngine, Guid>
    {
    }
}