using Customer.Grpc.Protos;

namespace Basket.API.GrpcServices
{
	public class CustomerGrpcService
	{
		private readonly CustomerProtoService.CustomerProtoServiceClient _protoService;
		public CustomerGrpcService(CustomerProtoService.CustomerProtoServiceClient protoService)
		{
			_protoService = protoService ?? throw new ArgumentNullException(nameof(protoService));
		}

		public async Task<CustomerModel> GetFullName(string userName)
		{
			try
			{
				var fullNameCustomerRequest = new GetCustomerRequest { UserName = userName };
				return await _protoService.GetCustomerAsync(fullNameCustomerRequest);
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
