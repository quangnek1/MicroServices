﻿using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;

namespace Product.API.Extensions
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers();
			services.Configure<RouteOptions>(options =>
			{
				options.LowercaseUrls = true;
			});
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.ConfigureProductDbContext(configuration);

			return services;

		}

		private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			var conntectionString = configuration.GetConnectionString("DefaultConnectionString");
			var builder = new MySqlConnectionStringBuilder(conntectionString);

			services.AddDbContext<ProductContext>(m => m.UseMySql(builder.ConnectionString,
			   ServerVersion.AutoDetect(builder.ConnectionString), e =>
			   {
				   e.MigrationsAssembly("Product.API");
				   e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
			   }));

			return services;
		}
	}
}
