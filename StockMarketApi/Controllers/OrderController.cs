using Facade.Contract;
using Facade.Contract.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StockMarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderQueryFacade orderQuery;

        public OrderController(IOrderQueryFacade orderQuery)
        {
            this.orderQuery = orderQuery;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderVM>> Get(long id)
        {
            return Ok(await orderQuery.Get(id).ConfigureAwait(false));
        }

        [HttpGet("{page}/{pageSize}/{currentPage}/{lastId}")]
        public async Task<ActionResult<IEnumerable<OrderVM>>> Get(
            int page,
            int pageSize,
            int currentPage,
            long lastId)
        {
            var result = await orderQuery
                .GetAllWithPaging(page, pageSize, currentPage, lastId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}