using Messages;

namespace MessageFacadeProvider
{
    public static class MessageMapperHelper
    {
        public static Side ToMessage(this Domain.Orders.Entities.Side side)
        {
            return side switch
            {
                Domain.Orders.Entities.Side.Buy => Side.Buy,
                _ => Side.Sell
            };
        }
    }
}
