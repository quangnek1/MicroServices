using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
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
	}
}
