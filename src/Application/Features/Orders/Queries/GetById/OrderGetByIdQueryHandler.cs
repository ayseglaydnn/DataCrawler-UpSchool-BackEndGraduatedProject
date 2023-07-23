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
	public class OrderGetByIdQueryHandler : IRequestHandler<OrderGetByIdQuery,OrderGetByIdDto>
	{
		private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;

        public OrderGetByIdQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }

		public async Task<OrderGetByIdDto> Handle(OrderGetByIdQuery request, CancellationToken cancellationToken)
		{

			var order = await _applicationDbContext.Orders.AsNoTracking()
				.Where(x => x.CreatedByUserId == _currentUserService.UserId)
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

			return MapOrderToGetByIdDto(order);
		}

		private OrderGetByIdDto MapOrderToGetByIdDto(Order order)
		{
				return new OrderGetByIdDto()
				{
					Id = order.Id,
					RequestedAmount = order.RequestedAmount,
					TotalFountAmount = order.TotalFountAmount,
					ProductCrawlType = (int)order.ProductCrawlType,
				};
		}
	}
}
