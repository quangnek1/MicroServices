using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDistributedCache _redisCaheServices;
		private readonly ISerializeServices _serializeServices;
		private readonly ILogger _logger;
		public BasketRepository(IDistributedCache redisCaheServices, ISerializeServices serializeServices, ILogger logger)
		{
			_redisCaheServices = redisCaheServices;
			_serializeServices = serializeServices;
			_logger = logger;
		}
		public async Task<bool> DeleteBasketFromUserName(string userName)
		{
			try
			{
				_logger.Information(messageTemplate: "BEGIN DeleteBasketFromUserName: {userName}", userName);
				await _redisCaheServices.RemoveAsync(userName);
				_logger.Information(messageTemplate: "END DeleteBasketFromUserName: {userName}", userName);
				return true;
			}
			catch (Exception e)
			{
				_logger.Error(messageTemplate: "Error DeleteBasketFromUserName" + e.Message);
				throw;
			}

		}

		public async Task<Cart?> GetBasketByUserName(string userName)
		{
			_logger.Information(messageTemplate: "BEGIN GetBasketByUserName: {userName}", userName);
			var basket = await _redisCaheServices.GetStringAsync(userName);
			_logger.Information(messageTemplate: "END GetBasketByUserName: {userName}", userName);

			return string.IsNullOrEmpty(basket) ? null :
				_serializeServices.Deserialize<Cart>(basket);
		}

		public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
		{
			_logger.Information(messageTemplate: "BEGIN UpdateBasket: {userName}", cart.UserName);
			if (options != null)
				await _redisCaheServices.SetStringAsync(key: cart.UserName, value: _serializeServices.Serialize(cart), options);
			else
				await _redisCaheServices.SetStringAsync(key: cart.UserName, value: _serializeServices.Serialize(cart));

			_logger.Information(messageTemplate: "END UpdateBasket: {userName}", cart.UserName);

			return await GetBasketByUserName(cart.UserName) ?? new Cart(cart.UserName);
		}
	}
}
