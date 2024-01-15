using Application.Contract.Commands;
using Domain;
using Facade.Contract;
using Facade.Contract.Model;
using Framework.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Controllers.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IOrderCommandFacade orderCommandFacade;
        private readonly IOrderQueryFacade orderQueryFacade;

        public OrdersController(ILogger<OrdersController> logger, IOrderCommandFacade orderCommandFacade, IOrderQueryFacade orderQueryFacade)
        {
            this.logger = logger;
            this.orderCommandFacade = orderCommandFacade;
            this.orderQueryFacade = orderQueryFacade;
        }

        [HttpPost]
        public async Task<ActionResult<ProcessedOrder>> ProcessOrder([FromBody] RegisterOrderVm registerOrderVm)
        {
            try
            {
                //TODO: can be done by a mapper  
                var command = new AddOrderCommand()
                {
                    Amount = registerOrderVm.Amount,
                    ExpDate = registerOrderVm.ExpireTime,
                    Side = registerOrderVm.Side,
                    Price = registerOrderVm.Price,
                    IsFillAndKill = registerOrderVm.IsFillAndKill,
                    CorrelationId = Guid.NewGuid(),

                };
                var result = await orderCommandFacade.ProcessOrder(command).ConfigureAwait(false);
                return CreatedAtAction(
                    "ProcessOrder",
                    "Orders",
                    null,

                    HATEOASlinkGenerator.ProcessOrderLink(result));
            }
            catch (Exception ex)
            {
                //TODO: must be optimized in future
                logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPut]
        public async Task<ActionResult<ProcessedOrder>> ModifyOrder([FromBody] ModifiedOrderVM modifyOrderVM)
        {
            var modifyCommand = new ModifyOrderCommand()
            {
                Amount = modifyOrderVM.Amount,
                Price = modifyOrderVM.Price,
                OrderId = modifyOrderVM.OrderId,
                ExpDate = modifyOrderVM.ExpDate,
                CorrelationId = Guid.NewGuid(),
            };
            //TODO: Also domain should notify the client about bad request or any other malfunctioning 
            var result = await orderCommandFacade.ModifyOrder(modifyCommand).ConfigureAwait(false);

            if (result == null)
                return BadRequest(modifyOrderVM);

            return Ok(HATEOASlinkGenerator.ModifyOrderLink(result));
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ProcessedOrder>> CancelOrder(long id)
        {
            try
            {
                var command = new CancelOrderCommand()
                {
                    Id = id,
                    CorrelationId = Guid.NewGuid(),
                };

                var result = await orderCommandFacade.CancelOrder(command).ConfigureAwait(false);

                if (result == null)
                    return BadRequest(command.Id);

                return Ok(HATEOASlinkGenerator.CancelOrderLink(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ProcessedOrder>> CancelAllOrders()
        {
            var command = new CancelAllOrderCommand()
            {
                CorrelationId = Guid.NewGuid()
            };

            var result = await orderCommandFacade.CancelAllOrders(command).ConfigureAwait(false);

            if (result == null)
                return BadRequest();

            return Ok(HATEOASlinkGenerator.CancelAllOrdersLink(result));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<OrderVM>> GetOrder(long id)
        {
            var result = await orderQueryFacade.Get(id).ConfigureAwait(false);
            return Ok(HATEOASlinkGenerator.GetOrderLink(result));
        }
        //TODO: pageSize and currentPage
        [HttpGet("{page:int}/{pageSize:int}/{currentPage:int}/{lastId:long}")]
        public async Task<ActionResult<PageResult<OrderVM>>> GetAllOrdersWithPaging(
            int page,
            int pageSize,
            int currentPage,
            long lastId)
        {
            var result = await orderQueryFacade
                .GetAllWithPaging(page, pageSize, currentPage, lastId)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}