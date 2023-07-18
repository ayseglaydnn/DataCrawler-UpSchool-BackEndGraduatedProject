using Application.Features.OrderEvent.Queries.GetAll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class OrderEventsController : ApiControllerBase
    {
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync(OrderEventGetAllQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
