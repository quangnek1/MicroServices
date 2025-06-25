using MediatR;
using Ordering.Application.Common.Models;
using Shared.Seedwork;

namespace Ordering.Application.Features.V1.Orders
{
	public class GetOrdersQuery : IRequest<ApiResult<List<OrderDto>>>
	{
		public string UserName { get; set; }

		public GetOrdersQuery(string useName)
		{
			UserName = useName ?? throw new ArgumentNullException(nameof(useName));
		}
	}
}
