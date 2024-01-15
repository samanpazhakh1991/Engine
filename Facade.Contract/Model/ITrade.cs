using Domain.Trades.Entities;

namespace Facade.Contract.Model
{
    public interface ITrade : ITradeInfo
    {
        long Id { get; }
    }
}