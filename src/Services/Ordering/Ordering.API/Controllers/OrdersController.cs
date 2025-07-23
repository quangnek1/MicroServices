using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using AutoMapper;
using Contracts.Messages;
using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Ordering.Domain.Entities;
using Shared.Seedwork;
using Shared.Services.Email;

namespace Ordering.API.Controllers
{
	[Route(template: "api/v1/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ISmtpEmailService _smtpEmailService;
		private readonly IMessageProducer _messageProducer;
		private readonly IOrderRepository _orderRepository;
		private readonly IMapper _mapper;
		public OrdersController(IMediator mediator, ISmtpEmailService smtpEmailService, IMessageProducer messageProducer, IOrderRepository orderRepository, IMapper mapper)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_smtpEmailService = smtpEmailService ?? throw new ArgumentNullException(nameof(_smtpEmailService));
			_messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(_messageProducer));
			_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(_orderRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
		}

		private static class RouteNames
		{
			public const string GetOrders = nameof(GetOrders);
			public const string CreateOrder = nameof(CreateOrder);
			public const string UpdateOrder = nameof(UpdateOrder);
			public const string DeleteOrder = nameof(DeleteOrder);
		}

		[HttpGet(template: "{userName}", Name = RouteNames.GetOrders)]
		[ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName([Required] string userName)
		{
			var query = new GetOrdersQuery(userName);
			var result = await _mediator.Send(query);

			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> TestMail()
		{
			var message = new MailRequest()
			{
				Body = "hello",
				Subject = "Test",
				ToAddress = "quangcntt96@gmail.com",
			};
			await _smtpEmailService.SendEmailAsync(message);

			return Ok();
		}

		[HttpPost(Name = RouteNames.CreateOrder)]
		[ProducesResponseType(type: typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ApiResult<long>>> CreateOrder([FromBody] CreateOrderCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		[HttpPut(template: "{id:long}", Name = RouteNames.UpdateOrder)]
		[ProducesResponseType(type: typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<OrderDto>> UpdateOrder([Required] long id, [FromBody] UpdateOrderCommand command)
		{
			command.SetId(id);
			var result = await _mediator.Send(command);

			return Ok(result);
		}

		[HttpDelete(template: "{id:long}", Name = RouteNames.DeleteOrder)]
		[ProducesResponseType(type: typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
		public async Task<ActionResult> DeleteOrder([Required] long id)
		{
			var command = new DeleteOrderCommand(id);
			await _mediator.Send(command);

			return NoContent();
		}

		// Message Queue
		//[HttpPost]
		//public async Task<ActionResult> CreateOrder(OrderDto orderDto)
		//{
		//	var order = _mapper.Map<Order>(orderDto);
		//	var addedOrder = await _orderRepository.CreateOrderDemo(order);
		//	await _orderRepository.SaveChangeAsync();
		//	var result = _mapper.Map<OrderDto>(addedOrder);
		//	_messageProducer.SendMessage(result);
		//	return Ok(result);
		//}
	}
}
