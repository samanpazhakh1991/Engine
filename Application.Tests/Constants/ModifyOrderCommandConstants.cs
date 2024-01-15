using Domain.Orders.Entities;
using System;

namespace Application.Tests.Constants
{
    public static class ModifyOrderCommandConstants
    {
        public static readonly int SOME_AMOUNT = 10;
        public static readonly DateTime SOME_EXPIRATION_DATE = new(2050, 1, 1);
        public static readonly bool SOME_IS_FILL_AND_KILL = false;
        public static readonly int SOME_PRICE = 10;
        public static readonly Side SOME_SIDE = Side.Buy;
        public static readonly int SOME_ORDER_PARENT_ID = 1;
        public static readonly int SOME_ORDER_ID = 0;
        public static readonly bool SOME_DOES_MATCH_IS_TRUE = true;
        public static readonly bool SOME_DOES_MATCH_IS_FALSE = false;
        public static readonly OrderStates SOME_ORDER_STATE = OrderStates.Register;
    }
}