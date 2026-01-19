using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories.Interfaces;
using EventBus.Messages.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Controllers
{
	[ApiController]
	[Route(template: "api/[controller]")]
	public class BasketsController : ControllerBase
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IMapper _mapper;
		private readonly StockItemGrpcService _stockItemGrpcService;
		//private readonly CustomerGrpcService _customerGrpcService;
		public BasketsController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint,
			IMapper mapper, StockItemGrpcService stockItemGrpcService/* CustomerGrpcService customerGrpcService*/)
		{
			_basketRepository = basketRepository;
			_publishEndpoint = publishEndpoint;
			_mapper = mapper;
			_stockItemGrpcService = stockItemGrpcService;
			//_customerGrpcService = customerGrpcService;
		}
		[HttpGet(template: "{userName}", Name = "GetBasket")]
		[ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Cart>> GetBasketByUserName([Required] string userName)
		{
			var result = await _basketRepository.GetBasketByUserName(userName);

			// Communicate with Customer.Grpc and check fullName of Customer
			//var customerModel = await _customerGrpcService.GetFullName(userName);
			//result.FullName = customerModel.FullName;

			return Ok(result ?? new Cart());
		}
		[HttpPost(Name = "UpdateBasket")]
		[ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart cart)
		{
			// Communicate with Inventory.Grpc and check quantity avalible of products
			//foreach (var item in cart.Items)
			//{
			//	var stock = await _stockItemGrpcService.GetStock(item.ItemNo);
			//	item.SetAvailableQuantity(stock.Quantity);
			//}

			// Communicate with Customer.Grpc and check fullName of Customer
			//var customerModel = await _customerGrpcService.GetFullName(cart.UserName);
			//cart.FullName = customerModel.FullName;

			var options = new DistributedCacheEntryOptions()
				.SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1))
				.SetSlidingExpiration(TimeSpan.FromMinutes(5));
			var result = await _basketRepository.UpdateBasket(cart, options);
			return Ok(result);
		}
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		[HttpDelete(template: "{userName}", Name = "DeleteBasket")]
		public async Task<ActionResult<bool>> DeleteBasket([Required] string userName)
		{
			var result = await _basketRepository.DeleteBasketFromUserName(userName);
			return Ok(result);
		}

		[Route(template: "[action]")]
		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Accepted)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
		{
			var basket = await _basketRepository.GetBasketByUserName(basketCheckout.UserName);
			if (basket == null) return NotFound();

			//publish checkout event to Event Bus Message 
			var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
			eventMessage.TotalPrice = basket.TotalPrice;

			await _publishEndpoint.Publish(eventMessage);

			// remove the basket
			await _basketRepository.DeleteBasketFromUserName(basket.UserName);

			return Accepted();
		}

	}
}
