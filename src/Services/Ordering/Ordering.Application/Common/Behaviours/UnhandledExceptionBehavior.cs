﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Common.Behaviours
{
	public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly ILogger<TResponse> _logger;
		public UnhandledExceptionBehavior(ILogger<TResponse> logger)
		{
			_logger = logger;
		}
		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			try
			{
				return await next();
			}
			catch (Exception ex)
			{

				var requestName = typeof(TRequest).Name;
				_logger.LogError(ex, message: "Application Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);
				throw;
			}
		}
	}
}
