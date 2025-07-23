using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Ordering.Application.Common.Mapping;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Shared.Seedwork;


namespace Ordering.Application.Features.V1.Orders;
public class UpdateOrderCommand : CreateOrderCommand, IRequest<ApiResult<OrderDto>>, IMapFrom<Order>
{
	public long Id { get; private set; }

	public void SetId(long id)
	{
		Id = id;
	}


	public void Mapping(Profile profile)
	{
		profile.CreateMap<UpdateOrderCommand, Order>()
			.ForMember(destinationMember: dest => dest.Status, memberOptions: opts => opts.Ignore())
			.IgnoreAllNonExising();

	}


}

