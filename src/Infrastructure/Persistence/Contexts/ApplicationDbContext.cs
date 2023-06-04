using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
	{
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderEvent> OrderEvents { get; set; }
		public DbSet<Product> Products { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Confiurations
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			//// Ignores 
			/// if we have more than one context, we should write these to ignore other context
			//modelBuilder.Ignore<User>();
			//modelBuilder.Ignore<Role>();
			//modelBuilder.Ignore<UserRole>();
			//modelBuilder.Ignore<RoleClaim>();
			//modelBuilder.Ignore<UserToken>();
			//modelBuilder.Ignore<UserClaim>();
			//modelBuilder.Ignore<UserLogin>();

			base.OnModelCreating(modelBuilder);
		}
	}
}
