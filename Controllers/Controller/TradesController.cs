using Facade.Contract;
using Facade.Contract.Model;
using Framework.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly ITradeQueryFacade tradeQueryFacade;

        public TradesController(ITradeQueryFacade tradeQueryFacade)
        {
            this.tradeQueryFacade = tradeQueryFacade;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeVM>>> GetAllTrades()
        {
            var result = await tradeQueryFacade.GetAllTrades().ConfigureAwait(false);
            return Ok(HATEOASlinkGenerator.GetAllTradeLink(result));
        }

        [HttpGet("{page:int}/{pageSize:int}/{currentPage:int}/{lastId:long}")]
        public async Task<ActionResult<PageResult<TradeVM>>> GetAllTradesWithPaging(int page, int pageSize, int currentPage, long lastId)
        {
            var result = await tradeQueryFacade.GetAllTradesWithPaging(page, pageSize, currentPage, lastId).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<TradeVM>> GetTrade(long id)
        {
            var result = await tradeQueryFacade.GetTrade(id).ConfigureAwait(false);
            return Ok(HATEOASlinkGenerator.GetTradeLink(result));
        }
    }
}