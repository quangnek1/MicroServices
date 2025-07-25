﻿using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Serilog;
using Shared.Seedwork;

namespace Ordering.Application.Features.V1.Orders
{
	public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ApiResult<List<OrderDto>>>
	{
		private readonly IMapper _mapper;
		private readonly IOrderRepository _orderRepository;
		private readonly ILogger _logger;

		public GetOrdersQueryHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
			_logger = logger ?? throw new ArgumentNullException(nameof(_logger));
		}
		private const string MethodName = "GetOrdersQueryHandler";

		public async Task<ApiResult<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
		{
			_logger.Information(messageTemplate: $"BEGIN: {MethodName} - UserName: {request.UserName}");
			var orderEntities = await _orderRepository.GetOrdersByUserName(request.UserName);
			var orderList = _mapper.Map<List<OrderDto>>(orderEntities);

			_logger.Information(messageTemplate: $"END: {MethodName} - UserName: {request.UserName}");

			return new ApiSuccessResult<List<OrderDto>>(orderList);
		}

	}
}
