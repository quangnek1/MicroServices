using Shared.DTOs.Customer;

namespace Customer.API.Services.Interfaces
{
	public interface ICustomerServices
	{
		Task<IResult> GetCustomerByUserNameAsync(string username);
		Task<IResult> GetCustomersAsync();
		Task<IResult> CreateCustomerAsync(CreateCustomerDto customerDto);
		Task<IResult> UpdateCustomerAsync(string userName, UpdateCustomerDto customerDto);
		Task<IResult> DeleteCustomerAsync(string userName);

	}
}
