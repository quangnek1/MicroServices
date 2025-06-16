using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories
{
	public class CustomerRepositoryAsync : RepositoryBaseAsync<Entities.Customer, int, CustomerContext>, ICustomerRepository
	{
		public CustomerRepositoryAsync(CustomerContext context, IUnitOfWork<CustomerContext> unitOfWork) : base(context, unitOfWork)
		{
		}

		public  Task<Entities.Customer> GetCustomerByUserNameAsync(string userName) =>
			 FindByCondition(x => x.UserName.Equals(userName)).SingleOrDefaultAsync();
	}
}
