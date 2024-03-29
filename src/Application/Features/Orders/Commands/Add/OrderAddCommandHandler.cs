﻿using Application.Common.Interfaces;
using Application.Common.Models.CrawlerLog;
using Application.Common.Models.Email;
using Application.Common.Models.Order;
using Application.Features.Orders.Queries.GetById;
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
        private readonly ICurrentUserService _currentUserService;


        public OrderAddCommandHandler(IApplicationDbContext applicationDbContext, IOrderHubService orderHubService,
            IEmailService emailService, ICrawlerService crawlerService, INotificationHubService notificationHubService, 
            ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _orderHubService = orderHubService;
            _emailService = emailService;
            _crawlerService = crawlerService;
            _notificationHubService = notificationHubService;
            _currentUserService = currentUserService;
        }

        public async Task<Response<Guid>> Handle(OrderAddCommand request, CancellationToken cancellationToken)
		{
            var orderId = Guid.NewGuid();

            var websiteUrl = "https://4teker.net/";
            var productDtos = await _crawlerService.ScrapeWebsiteAsync(websiteUrl, orderId.ToString(), request.RequestedAmount, request.ProductCrawlType, cancellationToken);

            var order = new Order()
			{
				Id = orderId,
                RequestedAmount = request.RequestedAmount,
                TotalFountAmount = productDtos.Count(),
                CreatedOn = DateTimeOffset.Now,
				ProductCrawlType = (ProductCrawlType)request.ProductCrawlType,
				CreatedByUserId = _currentUserService.UserId,
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


            //await _orderHubService.AddedAsync(new OrderDto(request.RequestedAmount, request.ProductCrawlType,request.Email, request.Name), cancellationToken);



            ////Send an Email
            //_emailService.SendEmailInformation(new SendEmailInformationDto()
            //{
            //    Email = request.Email,
            //    Name = request.Name,
            //});

            var notificationDto = new AppNotificationDto()
            {
                Message = $"Crawling process of order with {order.Id} Id is completed.",
                SentOn = DateTimeOffset.Now,
                UserId = _currentUserService.UserId,
            };

			//notification settings'e göre koşullandırılacak.
            await _notificationHubService.SendAppNotificationAsync(notificationDto, cancellationToken);


			return new Response<Guid>($"The crawl request with amount of \"{order.RequestedAmount}\" has been successfully created.", order.Id);
		}
	}
}
