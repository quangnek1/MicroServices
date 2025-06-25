using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Common.Behaviours
{
	public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly Stopwatch _time;
		private readonly ILogger<TRequest> _logger;
		public PerformanceBehaviour(ILogger<TRequest> logger)
		{
			_time = new Stopwatch();
			_logger = logger;
		}
		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			_time.Start();
			var response = await next();
			_time.Stop();

			var elapsedMilliseconds = _time.ElapsedMilliseconds;

			if (elapsedMilliseconds <= 500) return response;

			var requestName = typeof(TRequest).Name;
			_logger.LogWarning(message: "Application Long Running Request: {Name} ({ElapsedMiiliseconds} milliseconds) {@Request}",
				 requestName, elapsedMilliseconds, response);
			return response;
		}
	}
}
