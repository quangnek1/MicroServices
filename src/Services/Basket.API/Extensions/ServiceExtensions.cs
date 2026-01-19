using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using EventBus.Messages.IntergrationEvents.Interfaces;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Inventory.Grpc.Protos;
using MassTransit;
using Shared.Configurations;

namespace Basket.API.Extensions
{
	public static class ServiceExtensions
	{
		internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
		{
			var eventBusSettings = configuration.GetSection(key: nameof(EventBusSettings))
				.Get<EventBusSettings>();
			services.AddSingleton(eventBusSettings);

			var cacheSettings = configuration.GetSection(key: nameof(CacheSettings))
				.Get<CacheSettings>();
			services.AddSingleton(cacheSettings);

			var grpcSettings = configuration.GetSection(key: nameof(GrpcSettings))
				.Get<GrpcSettings>();
			services.AddSingleton(grpcSettings);

			return services;
		}
		public static IServiceCollection ConfigureServices(this IServiceCollection services) =>
			services.AddScoped<IBasketRepository, BasketRepository>()
				.AddTransient<ISerializeServices, SerializeServices>();

		public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
		{
			var settings = services.GetOptions<CacheSettings>(sectionName: "cacheSettings");
			if (string.IsNullOrEmpty(settings.ConnectionString))
			{
				throw new ArgumentNullException(nameof(settings), "Redis connection string is not configured.");
			}
			// Redis configuration
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = settings.ConnectionString;
				//	options.InstanceName = "BasketAPI_";
			});
		}

		public static IServiceCollection ConfigureGrpcServices(this IServiceCollection services)
		{
			var settings = services.GetOptions<GrpcSettings>(nameof(GrpcSettings));

			services.AddGrpcClient<StockProtoService.StockProtoServiceClient>
				(x => x.Address = new Uri(settings.StockUrl));
			services.AddScoped<StockItemGrpcService>();

			//services.AddGrpcClient<Customer.Grpc.Protos.CustomerProtoService.CustomerProtoServiceClient>
			//(x => x.Address = new Uri(settings.CustomerUrl));
			//services.AddScoped<CustomerGrpcService>();

			return services;
		}


		public static void ConfigureMassTransit(this IServiceCollection services)
		{
			var settings = services.GetOptions<EventBusSettings>(sectionName: "EventBusSettings");
			if (string.IsNullOrEmpty(settings.HostAddress))
			{
				throw new ArgumentNullException(nameof(settings), "EventBusSetting connection string is not configured.");
			}

			var mqConnection = new Uri(settings.HostAddress);
			services.AddSingleton(KebabCaseEndpointNameFormatter.Instance);
			services.AddMassTransit(config =>
			{
				config.UsingRabbitMq(configure: (ctx, cfg) =>
				{
					cfg.Host(mqConnection);
				});
				config.AddRequestClient<IBasketCheckoutEvent>();
			});
		}
	}
}
