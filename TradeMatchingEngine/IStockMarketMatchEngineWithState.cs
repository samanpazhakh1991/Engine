namespace Domain
{
    public interface IStockMarketMatchEngineWithState : IStockMarketMatchEngineProxy
    {
        void Open();

        void PreOpen();

        void Close();

        MarketState State { get; }

        public long Version { get; }
    }
}