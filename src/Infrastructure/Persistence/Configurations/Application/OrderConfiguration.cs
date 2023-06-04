using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Id
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();


            //RequestedAmount
            builder.Property(x => x.RequestedAmount).IsRequired();


            //TotalFountAmount
            builder.Property(x => x.TotalFountAmount).IsRequired();


            //ProductCrawlType (enum) 
            builder.Property(x => x.ProductCrawlType).IsRequired(); ;
			builder.Property(x=>x.ProductCrawlType).HasConversion<int>();

			/* Common Fields */

			// CreatedByUserId
			builder.Property(x => x.CreatedByUserId).IsRequired(false);
            builder.Property(x => x.CreatedByUserId).HasMaxLength(100);

            // CreatedOn
            builder.Property(x => x.CreatedOn).IsRequired();



            //Relationships
            builder.HasMany<Product>(x => x.Products)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);
            
            builder.HasMany<OrderEvent>(x => x.OrderEvents)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            

            builder.ToTable("Orders");
        }
    }
}
