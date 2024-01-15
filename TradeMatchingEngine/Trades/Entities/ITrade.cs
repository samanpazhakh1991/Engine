using Framework.Contracts.Common;

namespace Domain.Trades.Entities
{
    public interface ITrade : ITradeInfo, IEntity<long>
    {
    }
}