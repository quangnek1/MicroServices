using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Infrastructure.Common;

namespace Basket.API.Extensions
{
	public static class ServiceExtensions
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services) =>
			services.AddScoped<IBasketRepository, BasketRepository>()
				.AddTransient<ISerializeServices, SerializeServices>();

		public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
		{
			var redisConnectionString = configuration.GetSection("CacheSettings:ConnectionString").Value;	
			if (string.IsNullOrEmpty(redisConnectionString))
			{
				throw new ArgumentNullException(nameof(redisConnectionString), "Redis connection string is not configured.");
			}
			// Redis configuration
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = redisConnectionString;
			//	options.InstanceName = "BasketAPI_";
			});
		}
	}
}
