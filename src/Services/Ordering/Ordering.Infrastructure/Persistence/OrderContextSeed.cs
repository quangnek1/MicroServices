using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;

namespace Ordering.Infrastructure.Persistence
{
	public class OrderContextSeed
	{
		private readonly OrderContext _orderContext;
		private readonly ILogger _logger;
		public OrderContextSeed(OrderContext orderContext, ILogger logger)
		{
			_orderContext = orderContext ?? throw new ArgumentNullException(nameof(orderContext));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		public async Task InitialiseAsync()
		{
			try
			{
				if (_orderContext.Database.IsSqlServer())
				{
					await _orderContext.Database.MigrateAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, messageTemplate: "An error occurred seeding the DB.");
			}
		}

		public async Task SeedAsync()
		{
			try
			{
				if (!_orderContext.Orders.Any())
				{
					await TrySeedAsync();
					await _orderContext.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, messageTemplate: "An error occurred seeding the DB.");
				throw;
			}
		}

		public async Task TrySeedAsync()
		{
			if (!_orderContext.Orders.Any())
			{
				await _orderContext.Orders.AddRangeAsync(
					new Order
					{
						UserName = "customer1",
						TotalPrice = 100,
						FirstName = "John",
						LastName = "Doe",
						EmailAddress = "Customer1@local.com",
						ShippingAddress = "123 Main St, City, Country",
						InvoiceAddress = "123 Main St, City, Country"
					});
			}
		}

	}
}

