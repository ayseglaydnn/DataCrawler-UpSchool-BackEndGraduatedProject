using Application.Common.Models.Order;
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
	}
}
