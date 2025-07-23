using Contracts.Common.Interfaces;
using Contracts.Services;
using Infrastructure.Common;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Interfaces;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
	public static class ConfigureServices
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<OrderContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString(name: "DefaultConnectionString"),
				builder => builder.MigrationsAssembly(typeof(OrderContext).Assembly.FullName));
			});

			services.AddScoped<OrderContextSeed>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddScoped(serviceType: typeof(IUnitOfWork<>), implementationType: typeof(UnitOfWork<>));

			services.AddScoped(serviceType: typeof(ISmtpEmailService), implementationType: typeof(SmtpEmailService));
		//	services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
			return services;
		}
	}
}
