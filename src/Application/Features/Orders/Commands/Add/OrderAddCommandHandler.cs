using Application.Common.Interfaces;
using Application.Common.Models.Email;
using Application.Common.Models.Order;
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
	public class OrderAddCommandHandler : IRequestHandler<OrderAddCommand, Response<Guid>>
	{
		private readonly IApplicationDbContext _applicationDbContext;

		private readonly IOrderHubService _orderHubService;

		private readonly IEmailService _emailService;

		private readonly ICrawlerService _crawlerService;

		private readonly INotificationHubService _notificationHubService;
        public OrderAddCommandHandler(IApplicationDbContext applicationDbContext, IOrderHubService orderHubService, 
			IEmailService emailService, ICrawlerService crawlerService, INotificationHubService notificationHubService)
        {
            _applicationDbContext = applicationDbContext;
            _orderHubService = orderHubService;
            _emailService = emailService;
            _crawlerService = crawlerService;
            _notificationHubService = notificationHubService;
        }

        public async Task<Response<Guid>> Handle(OrderAddCommand request, CancellationToken cancellationToken)
		{
			var order = new Order()
			{
				Id = Guid.NewGuid(),
                RequestedAmount = request.RequestedAmount,
				CreatedOn = DateTimeOffset.Now,
				ProductCrawlType = (ProductCrawlType)request.ProductCrawlType,
				CreatedByUserId = null,
			};

			var orderEvent = new Domain.Entities.OrderEvent()
			{
				Id = Guid.NewGuid(),
				OrderId = order.Id,
				CreatedOn = DateTimeOffset.Now,
				Status = OrderStatus.BotStarted
			};

            await _applicationDbContext.Orders.AddAsync(order, cancellationToken);

            await _applicationDbContext.OrderEvents.AddAsync(orderEvent, cancellationToken);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var productDtos = await _crawlerService.ScrapeWebsiteAsync("https://finalproject.dotnet.gg/",order.Id.ToString(), request.RequestedAmount, request.ProductCrawlType, cancellationToken);

            foreach (var productDto in productDtos)
            {
				var product = new Product()
				{
					Name = productDto.Name,
					SalePrice = productDto.SalePrice,
					Price = productDto.Price,
					IsOnSale = productDto.IsOnSale,
					Picture = productDto.Picture,
					OrderId = order.Id,
				};
                await _applicationDbContext.Products.AddAsync(product, cancellationToken);
            }

            var orderEventNew = new Domain.Entities.OrderEvent()
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                CreatedOn = DateTimeOffset.Now,
                Status = OrderStatus.OrderCompleted,
            };

            await _applicationDbContext.OrderEvents.AddAsync(orderEventNew, cancellationToken);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);


			await _orderHubService.AddedAsync(new OrderDto(request.RequestedAmount,request.ProductCrawlType,request.Email,request.Name), cancellationToken);



            ////Send an Email
            //_emailService.SendEmailInformation(new SendEmailInformationDto()
            //{
            //    Email = request.Email,
            //    Name = request.Name,
            //});

			//notification settings'e göre koşullandırılacak.
            await _notificationHubService.SendAppNotificationAsync($"Crawling process of order with {order.Id} Id is completed.", cancellationToken);


			return new Response<Guid>($"The crawl request with amount of \"{order.RequestedAmount}\" has been successfully created.", order.Id);
		}
	}
}
