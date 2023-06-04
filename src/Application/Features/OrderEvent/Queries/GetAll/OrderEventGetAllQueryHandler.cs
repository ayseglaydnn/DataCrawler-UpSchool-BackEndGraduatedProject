using Application.Common.Interfaces;
using Application.Features.Products.Queries.GetAll;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderEvent.Queries.GetAll
{
    public class OrderEventGetAllQueryHandler : IRequestHandler<OrderEventGetAllQuery, List<OrderEventGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderEventGetAllQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<OrderEventGetAllDto>> Handle(OrderEventGetAllQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.OrderEvents.AsQueryable();

            dbQuery = dbQuery.Where(x => x.OrderId == request.OrderId);

            dbQuery = dbQuery.Include(x => x.Order);

            var orderEvents = await dbQuery.ToListAsync(cancellationToken);

            var orderEventDtos = MapOrderEventsGetAllDtos(orderEvents);

            return orderEventDtos.ToList();
        }
        private IEnumerable<OrderEventGetAllDto> MapOrderEventsGetAllDtos(List<Domain.Entities.OrderEvent> orderEvents)
        {
            foreach (var orderEvent in orderEvents)
            {
                yield return new OrderEventGetAllDto()
                {
                    OrderId= orderEvent.OrderId,
                    OrderStatus = orderEvent.Status.ToString(),
                    CreatedOn = DateTimeOffset.Now,
                };
            }
        }
    }
}
