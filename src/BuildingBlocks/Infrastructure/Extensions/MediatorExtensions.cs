using Contracts.Common.Events;
using Infrastructure.Common;
using MediatR;
using Serilog;

namespace Infrastructure.Extensions
{
	public static class MediatorExtensions
	{
		public static async Task DispatchDomainEventAsync(this IMediator mediator, List<BaseEvent> domainEvents, ILogger logger)
		{

			foreach (var domainEvent in domainEvents)
			{
				await mediator.Publish(domainEvent);
				var data = new SerializeServices().Serialize(domainEvent);
				logger.Information(messageTemplate: $"\n-----\nA Domain event has been published!\n" +
													$"Event: {domainEvent.GetType().Name} \n" +
													$"Event: {data}\n-----\n");
			}
		}
	}
}
