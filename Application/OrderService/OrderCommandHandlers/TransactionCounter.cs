using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderService.OrderCommandHandlers
{
    public class TransactionCounter : ITransactionCounter
    {
        public int Counter => _counter;
        private int _counter = 0;

        public int Decrement()
        {
            return Interlocked.Decrement(ref _counter);
        }

        public int Increment()
        {
            return Interlocked.Increment(ref _counter);
        }
    }
}