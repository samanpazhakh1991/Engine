using Domain.Trades.Entities;
using Framework.Contracts.GenericRepositories;

namespace Domain.Contract.Trades.Repository.Command
{
    public interface ITradeCommandRepository : ICommandRepository<Trade, ITrade, long>
    {
    }
}