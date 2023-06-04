using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductsGetAllQueryHandler : IRequestHandler<ProductsGetAllQuery, List<ProductsGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductsGetAllQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<ProductsGetAllDto>> Handle(ProductsGetAllQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Products.AsQueryable();

            dbQuery=dbQuery.Where(x => x.OrderId == request.OrderId);

            dbQuery = dbQuery.Include(x => x.Order);

            var products = await dbQuery.ToListAsync(cancellationToken);

            var productDtos = MapProductsGetAllDtos(products);

            return productDtos.ToList();
        }

        private IEnumerable<ProductsGetAllDto> MapProductsGetAllDtos(List<Product> products)
        {
            foreach(var product in products)
            {
                yield return new ProductsGetAllDto()
                {
                    Id = product.Id,
                    OrderId = product.OrderId,
                    Name = product.Name,
                    Picture = product.Picture,
                    IsOnSale = product.IsOnSale,
                    Price = product.Price,
                    SalePrice = product.SalePrice,
                    OrderRequestedAmount = product.Order.RequestedAmount,
                    OrderTotalFountAmount = product.Order.TotalFountAmount,
                    CrawlType = product.Order.ProductCrawlType.ToString(),
                };
            }
        }
    }
}
