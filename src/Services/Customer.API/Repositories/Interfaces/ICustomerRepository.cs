using Contracts.Common.Interfaces;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces
{
	public interface ICustomerRepository : IRepositoryBaseAsync<Entities.Customer, int, CustomerContext>
	{
		Task<Entities.Customer> GetCustomerByUserNameAsync(string userName);
		Task<IEnumerable<Entities.Customer>> GetCustomersAsync();
		Task CreateCustomer(Entities.Customer customer);
		Task UpdateCustomer(Entities.Customer customer);
		Task DeleteCustomer(Entities.Customer customer);
	}
}
