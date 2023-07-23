using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetById
{
	public class OrderGetByIdDto
	{
		public Guid Id { get; set; }
		public int RequestedAmount { get; set; }
		public int TotalFountAmount { get; set; }
		public int ProductCrawlType { get; set; }

	}
}
