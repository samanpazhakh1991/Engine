using Application.Contract.Commands;
using Domain;

namespace Facade.Contract
{
    public interface IOrderCommandFacade
    {
        Task<ProcessedOrder?> ProcessOrder(AddOrderCommand orderCommand);

        Task<ProcessedOrder?> ModifyOrder(ModifyOrderCommand orderCommand);

        Task<ProcessedOrder?> CancelOrder(CancelOrderCommand command);

        Task<ProcessedOrder?> CancelAllOrders(CancelAllOrderCommand obj);
    }
}