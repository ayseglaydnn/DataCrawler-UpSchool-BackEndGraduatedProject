using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetAll
{
	public class OrderGetAllQuery:IRequest<List<OrderGetAllDto>>
	{
        public string? CreatedByUserId { get; set; }

        public OrderGetAllQuery(string? createdByUserId)
        {
			CreatedByUserId = createdByUserId;

		}
    }
}
