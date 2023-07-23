using Application.Common.Models.Order;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Commands.Remove;
using Application.Features.Orders.Queries.GetAll;
using Application.Features.Orders.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Authorize]
	public class OrdersController : ApiControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> AddAsync(OrderAddCommand command)
		{
			return Ok(await Mediator.Send(command));
		}

        [HttpPost("CrawlerWorkerService")]
        public async Task<IActionResult> CrawlerWorkerServiceAsync(CrawlerWorkerServiceOrderDto orderAddDto)
        {
            return Ok();
        }

        [HttpPost("GetAll")]
		public async Task<IActionResult> GetAllAsync(OrderGetAllQuery query)
		{
			return Ok(await Mediator.Send(query));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByIdAsync(Guid id)
		{
			return Ok(await Mediator.Send(new OrderGetByIdQuery(id)));
		}

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteAsync(Guid orderId)
        {
			return Ok(await Mediator.Send(new OrderRemoveCommand(orderId)));
        }
    }          
}
