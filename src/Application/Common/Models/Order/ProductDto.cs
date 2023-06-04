using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Order
{
	public class ProductDto
	{
		public Guid OrderId { get; set; }
		//public Order Order { get; set; }
		public string Name { get; set; }
		public string Picture { get; set; }
		public bool IsOnSale { get; set; }
		public decimal Price { get; set; }
		public decimal? SalePrice { get; set; }
	}
}
