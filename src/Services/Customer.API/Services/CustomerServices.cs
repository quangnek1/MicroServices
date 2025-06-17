using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;

namespace Customer.API.Services
{
	public class CustomerServices : ICustomerServices
	{
		private readonly ICustomerRepository _customerRepository;
		public CustomerServices(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}
		public async Task<IResult> GetCustomerByUserNameAsync(string username) => Results.Ok(await _customerRepository.GetCustomerByUserNameAsync(username));

		public async Task<IResult> GetCustomersAsync() => Results.Ok(await _customerRepository.GetCustomersAsync());
	}
}
