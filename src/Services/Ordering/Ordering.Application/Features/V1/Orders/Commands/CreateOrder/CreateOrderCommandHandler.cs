using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.Seedwork;

namespace Ordering.Application.Features.V1.Orders;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
{
	private readonly IMapper _mapper;
	private readonly IOrderRepository _orderRepository;
	private readonly ILogger _logger;
	public CreateOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger)
	{
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
		_logger = logger ?? throw new ArgumentNullException(nameof(_logger));
	}
	private const string MethodName = "CreateOrderCommandHandler";
	public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
	{
		_logger.Information(messageTemplate: $"BEGIN: {MethodName} - UserName: {request.UserName}");

		var orderEntities = _mapper.Map<Order>(request);
		_orderRepository.CreateOrder(orderEntities);

		await _orderRepository.SaveChangeAsync();

		_logger.Information($"END: {MethodName} - Username: {request.UserName}");

		return new ApiSuccessResult<long>(orderEntities.Id);

	}
}
