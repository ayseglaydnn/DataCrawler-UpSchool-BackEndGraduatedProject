using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Remove
{
    internal class OrderRemoveCommandHandler : IRequestHandler<OrderRemoveCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IOrderHubService _orderHubService;

        public OrderRemoveCommandHandler(IOrderHubService orderHubService, IApplicationDbContext applicationDbContext)
        {
            _orderHubService = orderHubService;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Guid>> Handle(OrderRemoveCommand request, CancellationToken cancellationToken)
        {
            var order = await _applicationDbContext.Orders
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            _applicationDbContext.Orders.Remove(order);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            await _orderHubService.RemovedAsync(request.Id, cancellationToken);

            return new Response<Guid>("Order Deleted", request.Id);
        }
    }
}
