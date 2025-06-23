﻿using AutoMapper;
using Shared.DTOs.Customer;


namespace Customer.API
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<CreateCustomerDto, Entities.Customer>().ReverseMap();
			CreateMap<UpdateCustomerDto, Entities.Customer>().ReverseMap();
		}
	}
}
