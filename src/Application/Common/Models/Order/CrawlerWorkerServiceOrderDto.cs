using Application.Features.Orders.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Order
{
    public class CrawlerWorkerServiceOrderDto
    {
        public OrderGetByIdDto Order { get; set; }
        public string AccessToken { get; set; }

        public CrawlerWorkerServiceOrderDto(OrderGetByIdDto order, string accessToken)
        {
            Order = order;
            AccessToken = accessToken;
        }

    }
}
