using Application.Contract.Commands;
using System;

namespace Application.Tests
{
    public class TestCommand : ICommand
    {
        public Guid CorrelationId { get; set; }
    }
}