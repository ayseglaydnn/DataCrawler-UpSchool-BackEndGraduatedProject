using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Remove
{
    public class OrderRemoveCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }

        public OrderRemoveCommand(Guid id)
        {
            Id = id;
        }
    }
}
