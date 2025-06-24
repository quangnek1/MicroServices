using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application
{
	public static class ConfigureServices
	{
		//public static IServiceCollection AddApplicationServices(this IServiceCollection services)=>
		//	services
		//		.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly))
		//		.AddAutoMapper(typeof(ConfigureServices).Assembly)
		//		.AddValidatorsFromAssemblyContaining<ConfigureServices>()
		//		.AddTransient<IOrderingService, OrderingService>()
		//		.AddTransient<IOrderRepository, OrderRepository>()
		//		.AddTransient<IUnitOfWork, UnitOfWork>()
		//		.AddTransient<IEventBus, EventBus>();

	}
}
