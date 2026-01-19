using Customer.API.Services.Interfaces;
using Shared.DTOs.Customer;

namespace Customer.API.Controllers
{
	public static class CustomerController
	{
		public static void MapCustomerAPI(this WebApplication app)
		{

			app.MapGet(pattern: "/api/customers",
				handler: async (ICustomerServices customerServices) => await customerServices.GetCustomersAsync());

			app.MapGet(pattern: "/api/customers/{userName}",
			handler: async (string userName, ICustomerServices customerServices) =>
			{
				var result = await customerServices.GetCustomerByUserNameAsync(userName);
				return result != null ? result : Results.NotFound();
			});
			app.MapPost(pattern: "/api/customers",
				handler: async (CreateCustomerDto customerDto, ICustomerServices customerServices) =>
				{
					var result = await customerServices.CreateCustomerAsync(customerDto);
					return result != null ? result : Results.NotFound();
				});

			app.MapPut(pattern: "/api/customer/{userName}",
				handler: async (string userName, UpdateCustomerDto updateCustomerDto, ICustomerServices customerServices) =>
				{
					var result = await customerServices.UpdateCustomerAsync(userName, updateCustomerDto);
					return result != null ? result : Results.NotFound();
				});
			app.MapDelete(pattern: "/api/customer/{userName}",
				handler: async (string userName, ICustomerServices customerServices) =>
				{
					var result = await customerServices.DeleteCustomerAsync(userName);
					return result != null ? result : Results.NotFound();
				});
		}
	}
}
