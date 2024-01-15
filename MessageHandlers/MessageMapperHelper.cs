using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandlers
{
    public static class MessageMapperHelper
    {
        public static Domain.Orders.Entities.Side ToDomain(this Messages.Side side)
        {
            switch (side)
            {
                case Side.Buy: return Domain.Orders.Entities.Side.Buy;
                default: return Domain.Orders.Entities.Side.Sell;
            }
        }
    }
}
