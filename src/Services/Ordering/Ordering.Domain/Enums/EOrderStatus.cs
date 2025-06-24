namespace Ordering.Domain.Enums
{
	public enum EOrderStatus
	{
		New = 1, //start with 1, 0 is used for filter All =0
		Pending = 2,//order is pending, not any activities for a period of time.
		Paid = 3, //order is paid
		Shipping = 4, //order is shipping
		Fulfilled = 5, //order is fulfilled
	}
}
