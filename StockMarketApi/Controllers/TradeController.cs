using Facade.Contract;
using Facade.Contract.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace StockMarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ITradeQueryFacade tradeQuery;

        public TradeController(ITradeQueryFacade orderQuery)
        {
            this.tradeQuery = orderQuery;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TradeVM>> Get(long id)
        {
            return Ok(await tradeQuery.GetTrade(id).ConfigureAwait(false));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeVM>>> Get()
        {
            var result = await tradeQuery.GetAllTrades().ConfigureAwait(false);

            return Ok(result);
        }

        [HttpGet("{page}/{pageSize}/{currentPage}/{lastId}")]
        public async Task<ActionResult<IEnumerable<TradeVM>>> Get(
            int page,
            int pageSize,
            int currentPage,
            long lastId)
        {
            var result = await tradeQuery
                .GetAllTradesWithPaging(page, pageSize, currentPage, lastId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}