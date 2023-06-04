using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Add
{
	public class OrderAddCommand:IRequest<Response<Guid>>
	{
		public int RequestedAmount { get; set; } 		
		public int ProductCrawlType { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }

        //public int TotalFountAmount { get; set; }
    }
}
