using Application.Features.Orders.Queries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetById
{
	public class OrderGetByIdQuery : IRequest<List<OrderGetByIdDto>>
	{
		public Guid Id { get; set; }
		public string? CreatedByUserId { get; set; }

		public OrderGetByIdQuery(Guid id, string? createdByUserId)
		{
			Id = id;
			CreatedByUserId = createdByUserId;
		}
	}
}
