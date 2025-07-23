using Contracts.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Interfaces
{
	public interface IOrderRepository : IRepositoryBaseAsync<Order, long>
	{
		Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
		Task<Order> UpdateOrderAsync(Order order);
		void CreateOrder(Order order);
		void DeleteOrder(Order order);

		Task<Order> CreateOrderDemo(Order order);
	}
}
