using Application.Common.Interfaces;
using Application.Common.Models.Order;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Queries.GetAll;
using Application.Features.Orders.Queries.GetById;
using Domain.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
	public class OrderHubManager : IOrderHubService
	{
        
		private readonly IHubContext<OrderHub> _hubContext;

        public OrderHubManager(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task AddedAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync(SignalRMethodKeys.Orders.OrderAdded, orderDto, cancellationToken); //??
        }

        public Task RemovedAsync(Guid id, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync(SignalRMethodKeys.Orders.OrderRemoved, id, cancellationToken);
        }
        
    }
}
