using Application.Features.Orders.Queries.GetAll;
using Application.Features.Products.Queries.GetAll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync(ProductsGetAllQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
