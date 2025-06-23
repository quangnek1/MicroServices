using AutoMapper;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Shared.DTOs.Customer;
using Shared.DTOs.Product;

namespace Customer.API.Services
{
	public class CustomerServices : ICustomerServices
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly IMapper _mapper;
		public CustomerServices(ICustomerRepository customerRepository, IMapper mapper)
		{
			_customerRepository = customerRepository;
			_mapper = mapper;
		}
		#region CRUD
		
		public async Task<IResult> CreateCustomerAsync(CreateCustomerDto customerDto)
		{
			var customerEntity = await _customerRepository.GetCustomerByUserNameAsync(customerDto.UserName);
			if (customerEntity != null) return Results.BadRequest(customerEntity);

			var customer = _mapper.Map<Entities.Customer>(customerDto);
			await _customerRepository.CreateCustomer(customer);
			await _customerRepository.SaveChangeAsync();
			var result = _mapper.Map<CreateCustomerDto>(customer);
			return Results.Ok(result);
		}

		public async Task<IResult> GetCustomerByUserNameAsync(string username) => Results.Ok(await _customerRepository.GetCustomerByUserNameAsync(username));

		public async Task<IResult> GetCustomersAsync() => Results.Ok(await _customerRepository.GetCustomersAsync());

		public async Task<IResult> UpdateCustomerAsync(string userName, UpdateCustomerDto customerDto)
		{
			var existedEntity = await _customerRepository.GetCustomerByUserNameAsync(customerDto.UserName);
			if (existedEntity == null) return Results.BadRequest();

			var customer = _mapper.Map(source: customerDto, destination: existedEntity);
			await _customerRepository.UpdateCustomer(customer);
			await _customerRepository.SaveChangeAsync();
			var result = _mapper.Map<CreateCustomerDto>(customer);
			return Results.Ok(result);
		}
		public async Task<IResult> DeleteCustomerAsync(string userName)
		{
			var customer = await _customerRepository.GetCustomerByUserNameAsync(userName);
			if (customer == null) return Results.BadRequest();

			await _customerRepository.DeleteCustomer(customer);
			await _customerRepository.SaveChangeAsync();
			return Results.NoContent();
		}
		#endregion
	}
}
