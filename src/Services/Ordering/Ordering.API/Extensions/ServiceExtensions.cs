using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.Application.IntergrationEvents.EventHandler;
using Shared.Configurations;

namespace Ordering.API.Extensions
{
	public static class ServiceExtensions
	{
		internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
		{
			var emailSettings = configuration.GetSection(key: nameof(SMTPEmailSetting))
				.Get<SMTPEmailSetting>();
			services.AddSingleton(emailSettings);

			var eventBusSettings = configuration.GetSection(key: nameof(EventBusSettings))
				.Get<EventBusSettings>();
			services.AddSingleton(eventBusSettings);

			return services;
		}
		public static void ConfigureMassTransit(this IServiceCollection services)
		{
			var settings = services.GetOptions<EventBusSettings>(sectionName: nameof(EventBusSettings));
			if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
				throw new ArgumentNullException(paramName: "EvenBusSetting is not configured.");

			var mqConnection = new Uri(settings.HostAddress);
			services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
			services.AddMassTransit(config =>
			{
				config.AddConsumersFromNamespaceContaining<BasketCheckoutEventHandler>();
				config.UsingRabbitMq(configure: (ctx, cfg) =>
				{
					cfg.Host(mqConnection);
					//cfg.ReceiveEndpoint("basket-checkout-queue", c =>
					//{
					//	c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
					//});
					cfg.ConfigureEndpoints(ctx);
				});
			});
		}
	}

}
