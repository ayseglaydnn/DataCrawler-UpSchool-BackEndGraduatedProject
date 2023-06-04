using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderEvent.Queries.GetAll
{
    public class OrderEventGetAllDto
    {
        public Guid OrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
