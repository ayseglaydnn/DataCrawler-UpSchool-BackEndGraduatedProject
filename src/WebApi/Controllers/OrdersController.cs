using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Queries.GetAll;
using Application.Features.Orders.Queries.GetById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	public class OrdersController : ApiControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> AddAsync(OrderAddCommand command)
		{
			return Ok(await Mediator.Send(command));
		}

		[HttpPost("GetAll")]
		public async Task<IActionResult> GetAllAsync(OrderGetAllQuery query)
		{
			return Ok(await Mediator.Send(query));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByIdAsync(Guid id)
		{
			return Ok(await Mediator.Send(new OrderGetByIdQuery(id,null)));
		}
	}          
}
