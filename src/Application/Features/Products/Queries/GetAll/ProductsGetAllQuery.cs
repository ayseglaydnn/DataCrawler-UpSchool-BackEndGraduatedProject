﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductsGetAllQuery:IRequest<List<ProductsGetAllDto>>
    { 
        public Guid OrderId { get; set; }

        public ProductsGetAllQuery(Guid orderId)
        {
            OrderId = orderId;
        }

    }
}
