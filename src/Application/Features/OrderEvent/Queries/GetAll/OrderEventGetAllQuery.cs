using Application.Features.Products.Queries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderEvent.Queries.GetAll
{
    public class OrderEventGetAllQuery : IRequest<List<OrderEventGetAllDto>>
    {
        public Guid OrderId { get; set; }

        public OrderEventGetAllQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
