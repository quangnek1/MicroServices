namespace Customer.API.Services.Interfaces
{
	public interface ICustomerServices
	{
		Task<IResult> GetCustomerByUserNameAsync(string username);
		Task<IResult> GetCustomersAsync();
	}
}
