﻿using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Customer
{
	public class CustomerDto
	{
		public int Id { get; set; }
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
