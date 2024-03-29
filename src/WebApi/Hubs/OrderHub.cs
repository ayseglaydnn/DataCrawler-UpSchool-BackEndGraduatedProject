﻿using Application.Common.Models.Order;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Queries.GetAll;
using Application.Features.Orders.Queries.GetById;
using Domain.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
	public class OrderHub:Hub
	{
        private ISender? _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderHub(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        protected ISender Mediator => _mediator ??= _contextAccessor.HttpContext.RequestServices.GetRequiredService<ISender>();
        [Authorize]
        public async Task<Guid> AddNewOrder(OrderAddCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var accessToken = Context.GetHttpContext().Request.Query["access_token"];

                var result = await Mediator.Send(command);

                var orderGetById = await Mediator.Send(new OrderGetByIdQuery(result.Data));

                await Clients.All.SendAsync("NewOrderAdded",
                    new CrawlerWorkerServiceOrderDto(orderGetById, accessToken));

                return result.Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
