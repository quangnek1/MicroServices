using System.ComponentModel.DataAnnotations;
using System.Net;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Controllers
{
	[ApiController]
	[Route(template: "api/[controller]")]
	public class BasketsController : ControllerBase
	{
		private readonly IBasketRepository _basketRepository;
		public BasketsController(IBasketRepository basketRepository)
		{
			_basketRepository = basketRepository;
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
	}
}
