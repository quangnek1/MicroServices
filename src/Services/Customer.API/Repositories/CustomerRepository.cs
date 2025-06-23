using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories
{
	public class CustomerRepository : RepositoryBaseAsync<Entities.Customer, int, CustomerContext>, ICustomerRepository
	{
		public CustomerRepository(CustomerContext context, IUnitOfWork<CustomerContext> unitOfWork) : base(context, unitOfWork)
		{
		}

		public Task<Entities.Customer> GetCustomerByUserNameAsync(string userName) =>
			 FindByCondition(x => x.UserName.Equals(userName)).SingleOrDefaultAsync();

		public async Task<IEnumerable<Entities.Customer>> GetCustomersAsync() => await FindAll().ToListAsync();
		public Task CreateCustomer(Entities.Customer customer) => CreateAsync(customer);
		public Task UpdateCustomer(Entities.Customer customer) => UpdateAsync(customer);
		public Task DeleteCustomer(Entities.Customer customer) => DeleteAsync(customer);
	}
}
