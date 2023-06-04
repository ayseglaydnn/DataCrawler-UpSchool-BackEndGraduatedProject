using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Id
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            // Name
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(150);

            //Picture
            builder.Property(x => x.Picture).IsRequired();
            builder.Property(x => x.Picture).HasMaxLength(1000);

            //IsOnSale
            builder.Property(x => x.IsOnSale).IsRequired();
            //?builder.Property(x => x.IsOnSale).HasDefaultValueSql("0");

            //Price
            builder.Property(x => x.Price).IsRequired();


            //SalePrice
            builder.Property(x => x.SalePrice).IsRequired(false);



            /* Common Fields */

            // CreatedByUserId
            builder.Property(x => x.CreatedByUserId).IsRequired(false);
            builder.Property(x => x.CreatedByUserId).HasMaxLength(100);

            // CreatedOn
            builder.Property(x => x.CreatedOn).IsRequired();


            builder.ToTable("Products");


            ////Relationships
            //builder.HasOne<Order>(x => x.Order)
            //    .WithMany(x => x.Products)
            //    .HasForeignKey(x => x.OrderId);
        }
    }
}
