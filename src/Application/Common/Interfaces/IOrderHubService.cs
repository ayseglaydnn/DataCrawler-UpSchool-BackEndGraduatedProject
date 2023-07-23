using Application.Common.Models.Order;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Queries.GetById;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
	public interface IOrderHubService
	{
        Task AddedAsync(OrderDto orderDto, CancellationToken cancellationToken);
        Task RemovedAsync(Guid id, CancellationToken cancellationToken);
    }
}
