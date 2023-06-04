using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Order
{
    public class OrderDto
    {
        public int RequestedAmount { get; set; }
        public int ProductCrawlType { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public OrderDto(int requestedAmount, int productCrawlType, string email, string name)
        {
            RequestedAmount = requestedAmount;
            ProductCrawlType = productCrawlType;
            Email = email;
            Name = name;
            CreatedOn = DateTimeOffset.Now;
        }

    }
}
