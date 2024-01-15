
using Facade.Contract.Model;

namespace SpecFlowTest
{
    public class StockMarketClient
    {
        public StockMarketClient(string baseAddress)
        {
            BaseAddress = baseAddress;
            HttpClientWorker.AddConnection(baseAddress);
        }

        public string BaseAddress { get; }

        public Task<GetAllTradesResult> GetTrades()
        {
            return HttpClientWorker.Execute<GetAllTradesResult>($"{BaseAddress}/trades", HttpMethod.Get);
        }
        public Task<GetTradeByIdResult> GetTradeById(long id)
        {
            var trade = HttpClientWorker.Execute<GetTradeByIdResult>($"{BaseAddress}/trades/{id}", HttpMethod.Get);
            return trade;
        }
        public Task CancelAllOrders()
        {
            return HttpClientWorker.Execute($"{BaseAddress}/orders", HttpMethod.Delete);
        }
        public Task CancelOrder(long id)
        {
            return HttpClientWorker.Execute($"{BaseAddress}/orders/{id}", HttpMethod.Delete);
        }
        public Task<GetResult> GetOrderById(long id)
        {
            return HttpClientWorker.Execute<GetResult>($"{BaseAddress}/orders/{id}", HttpMethod.Get);
        }
        public Task<PostResult> ProcessOrder(OrderVM order)
        {
            return HttpClientWorker.Execute<OrderVM, PostResult>($"{BaseAddress}/orders", HttpMethod.Post, order);
        }
        public Task<PutResult> ModifyOrder(ModifiedOrderVM order)
        {
            return HttpClientWorker.Execute<ModifiedOrderVM, PutResult>($"{BaseAddress}/orders", HttpMethod.Put, order);
        }
    }
}