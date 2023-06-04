using Application.Common.Models.Order;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
	public interface ICrawlerService
	{
		Task<IEnumerable<ProductDto>> ScrapeWebsiteAsync(string websiteUrl, string orderId, int requestedAmount, int crawlType, CancellationToken cancellationToken);
	}
}
