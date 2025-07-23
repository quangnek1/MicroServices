using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
	public class OrderRepository : RepositoryBaseAsync<Order, long, OrderContext>, IOrderRepository
	{
		public OrderRepository(OrderContext context, IUnitOfWork<OrderContext> unitOfWork) : base(context, unitOfWork)
		{
		}


		public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName) =>
		await FindByCondition(expression: x => x.UserName.Equals(userName)).ToListAsync();

		public void CreateOrder(Order order) => CreateAsync(order);

		public async Task<Order> UpdateOrderAsync(Order order)
		{
			await UpdateAsync(order);
			return order;
		}

		public void DeleteOrder(Order order) => DeleteAsync(order);

		public async Task<Order> CreateOrderDemo(Order order)
		{
			await CreateAsync(order);

			return order;
		}
	}
}
