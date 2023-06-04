using Application.Common.Interfaces;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
	public static class ConfigureServices
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string wwwrootPath)
		{


			var connectionString = configuration.GetConnectionString("MariaDb");

			// DbContexts

			services.AddDbContext<ApplicationDbContext>(opt => opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

			services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());


			// Scoped Services
			services.AddScoped<ICrawlerService,CrawlerManager>();


			//Singleton Services
			services.AddSingleton<IEmailService>(new EmailManager(wwwrootPath));


			return services;
		}
	}
}
