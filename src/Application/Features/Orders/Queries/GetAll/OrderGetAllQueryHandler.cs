using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetAll
{
	public class OrderGetAllQueryHandler : IRequestHandler<OrderGetAllQuery, List<OrderGetAllDto>>
	{
		private readonly IApplicationDbContext _applicationDbContext;

		public OrderGetAllQueryHandler(IApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public async Task<List<OrderGetAllDto>> Handle(OrderGetAllQuery request, CancellationToken cancellationToken)
		{
			var dbQuery = _applicationDbContext.Orders.AsQueryable();

			dbQuery = dbQuery.Where(x=>x.CreatedByUserId == request.CreatedByUserId);


			var orders = await dbQuery.ToListAsync(cancellationToken);

			var orderDtos = MapOrdersToGetAllDtos(orders);

			return orderDtos.ToList();
		}

		private IEnumerable<OrderGetAllDto> MapOrdersToGetAllDtos(List<Order> orders)
		{

			foreach (var order in orders)
			{
				yield return new OrderGetAllDto()
				{
					Id = order.Id,
					CreatedByUserId = order.CreatedByUserId,
					RequestedAmount = order.RequestedAmount,
					TotalFountAmount = order.TotalFountAmount,
					ProductCrawlType = order.ProductCrawlType.ToString(),
					CreatedOn = order.CreatedOn.ToString(),
				}; 
			}
		}
	}
}
