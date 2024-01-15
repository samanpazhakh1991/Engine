using Application.Contract.Commands;
using Domain;

namespace Application.Contract.CommandHandlerContracts
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        Task<ProcessedOrder?> Handle(T command);
    }
}