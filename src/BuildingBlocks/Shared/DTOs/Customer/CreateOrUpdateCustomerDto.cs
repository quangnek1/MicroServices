using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Customer
{
	public class CreateOrUpdateCustomerDto
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		[EmailAddress]
		public string EmailAddress { get; set; }
	}
}
