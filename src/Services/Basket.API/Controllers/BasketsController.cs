using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Basket.API.Entities;
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
		public BasketsController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint, IMapper mapper)
		{
			_basketRepository = basketRepository;
			_publishEndpoint = publishEndpoint;
			_mapper = mapper;
		}
		[HttpGet(template: "{userName}", Name = "GetBasket")]
		[ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Cart>> GetBasketByUserName([Required] string userName)
		{
			var result = await _basketRepository.GetBasketByUserName(userName);
			return Ok(result ?? new Cart());
		}
		[HttpPost(Name = "UpdateBasket")]
		[ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart cart)
		{
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
