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
    public class OrderEventConfiguration : IEntityTypeConfiguration<OrderEvent>
    {
        public void Configure(EntityTypeBuilder<OrderEvent> builder)
        {
            // Id
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

			//OrderStatus (enum)
			builder.Property(x => x.Status).IsRequired(); ;
			builder.Property(x => x.Status).HasConversion<int>();


			/* Common Fields */

			// CreatedByUserId
			builder.Property(x => x.CreatedByUserId).IsRequired(false);
            builder.Property(x => x.CreatedByUserId).HasMaxLength(100);

            // CreatedOn
            builder.Property(x => x.CreatedOn).IsRequired();

            builder.ToTable("OrderEvents");

            ////Relationships
            //builder.HasOne<Order>(x => x.Order)
            //    .WithMany(x => x.OrderEvents)
            //    .HasForeignKey(x => x.OrderId);
        }
    }
}
