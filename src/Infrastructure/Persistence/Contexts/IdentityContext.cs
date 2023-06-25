using Domain.Entities;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.DevTools.V113.FedCm;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public class IdentityContext:IdentityDbContext<User,Role,string,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Ignores
            modelBuilder.Ignore<Order>();
            modelBuilder.Ignore<OrderEvent>();
            modelBuilder.Ignore<Product>();


            base.OnModelCreating(modelBuilder);
        }
    }
}
