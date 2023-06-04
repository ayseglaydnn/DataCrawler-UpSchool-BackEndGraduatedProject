using Application.Common.Interfaces;
using Application.Features.Orders.Queries.GetAll;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetById
{
	public class OrderGetByIdQueryHandler : IRequestHandler<OrderGetByIdQuery, List<OrderGetByIdDto>>
	{
		private readonly IApplicationDbContext _applicationDbContext;

		public OrderGetByIdQueryHandler(IApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public async Task<List<OrderGetByIdDto>> Handle(OrderGetByIdQuery request, CancellationToken cancellationToken)
		{
			var dbQuery = _applicationDbContext.Orders.AsQueryable();

			dbQuery = dbQuery.Where(x => x.Id == request.Id);


			var orders = await dbQuery.ToListAsync(cancellationToken);

			var orderDtos = MapOrdersToGetByIdDtos(orders);

			return orderDtos.ToList();
		}

		private IEnumerable<OrderGetByIdDto> MapOrdersToGetByIdDtos(List<Order> orders)
		{

			foreach (var order in orders)
			{
				yield return new OrderGetByIdDto()
				{
					Id = order.Id,
					RequestedAmount = order.RequestedAmount,
					TotalFountAmount = order.TotalFountAmount,
					ProductCrawlType = order.ProductCrawlType.ToString(),
				};
			}
		}
	}
}
