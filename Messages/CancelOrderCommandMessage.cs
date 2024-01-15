using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class CancelOrderCommandMessage : ICommand
    {
        public long Id { get; set; }

        public Guid CorrelationId { get; set; }
    }
}