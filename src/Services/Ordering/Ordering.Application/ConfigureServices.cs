using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Behaviours;

namespace Ordering.Application
{
	public static class ConfigureServices
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
			services
				.AddAutoMapper(typeof(ConfigureServices).Assembly)
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
				.AddMediatR(Assembly.GetExecutingAssembly())
				.AddTransient(serviceType: typeof(IPipelineBehavior<,>), implementationType: typeof(UnhandledExceptionBehavior<,>))
				.AddTransient(serviceType: typeof(IPipelineBehavior<,>), implementationType: typeof(PerformanceBehaviour<,>))
				.AddTransient(serviceType: typeof(IPipelineBehavior<,>), implementationType: typeof(ValidationBehaviour<,>))
			;
	}
}
